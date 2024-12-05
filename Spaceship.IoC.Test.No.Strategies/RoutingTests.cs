using Hwdtech;
using Spaceship__Server;
using Moq;
using System.Collections.Concurrent;

namespace Spaceship.IoC.Test.No.Strategies;

public class RoutingTests
{
    [Fact]
    public void MainPositiveRoutingTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); 

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Message deserialize", (object[] args) => {
            Spaceship__Server.ICommand cmd = (Spaceship__Server.ICommand) args[0];

            return cmd;
        }).Execute(); 


        InitiateThreadDependenciesStrategy.Run("1");

        BlockingCollection<Spaceship__Server.ICommand> queue1 = new();
        BlockingCollection<Spaceship__Server.ICommand> orderQueue1 = new();
        
        ISender snd1 = new SenderAdapter(orderQueue1);
        ISender internalSnd1 = new SenderAdapter(queue1);
        

        IReciver rec1 = new RecieverAdapter(queue1);
        IReciver orderRec1 = new RecieverAdapter(orderQueue1);


        Dictionary<string, ISender> internalDicts = new(){{"1", internalSnd1}};
        Dictionary<string, ISender> routeDict = new(){{"1", snd1}};

        Spaceship__Server.IRouter router = new DictRouter(routeDict);

        Dictionary<string, object> ValueDictionary1 = new(){{"type", "StartMove"}, {"gameid", "2.1"}, {"objid", "obj123"}, {"thread", "2"}, {"velocity", 1}};
        Dictionary<string, object> ValueDictionary2 = new(){{"type", "StopMove"}, {"gameid", "2.1"}, {"objid", "obj123"}, {"thread", "2"}};
        Dictionary<string, object> ValueDictionary3 = new(){{"type", "StartRotate"}, {"gameid", "2.1"}, {"objid", "obj123"}, {"thread", "2"}};
        Dictionary<string, object> ValueDictionary4 = new(){{"type", "Shoot"}, {"gameid", "2.1"}, {"objid", "obj123"}, {"thread", "2"}};

        Assert.Empty(orderQueue1);

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetOrderSenderByThreadId", (object[] args) => {
            return routeDict[(string)args[0]];
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInternalSenderByThreadId", (object[] args) => {
            return internalDicts[(string)args[0]];
        }).Execute();

        Hwdtech.IoC.Resolve<Spaceship__Server.ICommand>("Send Order", new ActionCommand(() => {}));
        router.Route("1", ValueDictionary1);
        router.Route("1", ValueDictionary2);
        router.Route("1", ValueDictionary3);
        router.Route("1", ValueDictionary4);

        Assert.NotEmpty(orderQueue1);


        orderRec1.Receive().Execute();
        orderRec1.Receive().Execute();
        orderRec1.Receive().Execute();
        orderRec1.Receive().Execute();

        Assert.Empty(orderQueue1);
    }

    [Fact]
    public void EndpointInitTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        InitiateThreadDependenciesStrategy.Run("1");

        var newScope = Hwdtech.IoC.Resolve<object>("Scopes.Current");

        Assert.NotEqual(scope, newScope);
    }

    [Fact]
    public void RoutingThrowsTest()
    {
        Mock<ISender> snd = new();

        snd.Setup(s => s.Send(It.IsAny<object>)).Throws<Exception>();
        
        Dictionary<string, ISender> routeDict = new(){{"1", snd.Object}};

        Spaceship__Server.IRouter router = new DictRouter(routeDict);

        Dictionary<string, object> ValueDictionary1 = new(){{"type", "StartMove"}, {"gameid", "2.1"}, {"objid", "obj123"}, {"thread", "2"}, {"velocity", 1}};

        Assert.False(router.Route("1", ValueDictionary1));

    }
}
