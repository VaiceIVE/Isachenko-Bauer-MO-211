using Grpc.Core;
using gRPC;
using Spaceship__Server;
using System.Collections.Concurrent;
using Hwdtech;
namespace gRPC.Services;

public class GreeterService : Greeter.GreeterBase
{

    private Spaceship__Server.IRouter _router;

    private object scope;

    public GreeterService()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        this.scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", this.scope).Execute();  
        
        BlockingCollection<Spaceship__Server.ICommand> queue1 = new();
        BlockingCollection<Spaceship__Server.ICommand> queue2 = new();
        BlockingCollection<Spaceship__Server.ICommand> orderQueue1 = new();
        BlockingCollection<Spaceship__Server.ICommand> orderQueue2 = new();
        
        ISender snd1 = new SenderAdapter(orderQueue1);
        ISender snd2 = new SenderAdapter(orderQueue2);
        ISender internalSnd1 = new SenderAdapter(queue1);
        ISender internalSnd2 = new SenderAdapter(queue2);
        

        IReciver rec1 = new RecieverAdapter(queue1);
        IReciver rec2 = new RecieverAdapter(queue2);
        IReciver orderRec1 = new RecieverAdapter(orderQueue1);
        IReciver orderRec2 = new RecieverAdapter(orderQueue2);

        MyThread thread1 = new(rec1, orderRec1);
        MyThread thread2 = new(rec2, orderRec2);

        Dictionary<string, ISender> internalDicts = new(){{"1", internalSnd1}, {"2", internalSnd2}};
        Dictionary<string, ISender> routeDict = new(){{"1", snd1}, {"2", snd2}};

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Message deserialize", (object[] args) => {
            Spaceship__Server.ICommand cmd = (Spaceship__Server.ICommand) args[0];

            return cmd;
        }).Execute(); 

        snd1.Send(new ActionCommand(() => {
            InitiateThreadDependenciesStrategy.Run("1");
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInternalSenderByThreadId", (object[] args) => {
            return internalDicts[(string)args[0]];
        }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetOrderSenderByThreadId", (object[] args) => {
            return routeDict[(string)args[0]];
        }).Execute();
        })).Execute();

        snd2.Send(new ActionCommand(() => {
            InitiateThreadDependenciesStrategy.Run("2");
            Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInternalSenderByThreadId", (object[] args) => {
            return internalDicts[(string)args[0]];
        }).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetOrderSenderByThreadId", (object[] args) => {
            return routeDict[(string)args[0]];
        }).Execute();
        })).Execute();

        thread1.Start();
        thread2.Start();

        Spaceship__Server.IRouter router = new DictRouter(routeDict);

        _router = router;
    }

    public override Task<StatusReply> SayHello(gRPCMessage request, ServerCallContext context)
    {
        string status = "";

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", this.scope).Execute();  

        Dictionary<string, object> data = (Dictionary<string, object>) ProtobufMapperStrategy.Run(request.Props);

        if(_router.Route(((string) data["gameid"]).Split('.')[0], data))
        {
            status = "good";
        }
        else{
            status = "bad";
        }

        return Task.FromResult(new StatusReply
        {
            Status = status
        });
    }
}
