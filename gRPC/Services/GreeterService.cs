using Grpc.Core;
using gRPC;
using Spaceship__Server;
namespace gRPC.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        Dependencies.Run();
        Hwdtech.IoC.Resolve<MyThread>("Create and Start Thread", "2");
        _logger = logger;
    }

    public override Task<StatusReply> SayHello(gRPCMessage request, ServerCallContext context)
    {
        string status = "";

        try{
            Hwdtech.IoC.Resolve<Spaceship__Server.ICommand>("Deserialize Message to Command", Hwdtech.IoC.Resolve<Dictionary<string, object>>("Map protobuf to dict", request.Props)).Execute();
            status = "good";
        }
        catch{
            status = "Bad";
        }

        

        return Task.FromResult(new StatusReply
        {
            Status = status
        });
    }
}
