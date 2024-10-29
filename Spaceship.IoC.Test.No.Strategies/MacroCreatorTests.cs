using Hwdtech;
using Moq;
using Spaceship__Server;

namespace Spaceship.IoC.Test.No.Strategies;


public class MacroBuilderTest
{
    [Fact]
    public void InitialTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.Movable", (object[] args) => 
        {
            MovableAdapter adp = new MovableAdapter(args);
            return adp;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ContiniousMovement.Get.Dependencies", (object[] args) =>
        {
            List<string> deps = new List<string>{"MoveCommand"};
            return deps;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "IoC.MoveCommand", (object[] args) =>
        {
            return (Spaceship__Server.ICommand) new MoveCommand(Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.Movable", args));;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "IoC.CreateMacro", (object[] args) =>
        {
            MacroCreator creator = new();
            return creator.CreateMacro(args);
        }).Execute();

        Mock<IUObject> obj = new();

        Mock<IUObject> _obj = new();

        Queue<Spaceship__Server.ICommand> _queue = new();

        _obj.Setup(o => o.get_property("Velocity")).Returns((object) new Vector(1, 1));

        _obj.Setup(o => o.get_property("Position")).Returns((object) new Vector(0, 0));

        obj.Setup(o => o.get_property("Object")).Returns((object) _obj.Object);

        obj.Setup(o => o.get_property("Queue")).Returns((object) _queue);

        Hwdtech.IoC.Resolve<Spaceship__Server.ICommand>("IoC.CreateMacro", "ContiniousMovement", obj.Object).Execute();

        Assert.Single(_queue);
    }

    [Fact]
    public void GetSpeedTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.Movable", (object[] args) => 
        {
            MovableAdapter adp = new MovableAdapter(args);
            return adp;
        }).Execute();

        Mock<IUObject> obj = new();

        obj.Setup(o => o.get_property("Position")).Returns(new Vector(2));

        obj.Setup(o => o.get_property("Velocity")).Returns(new Vector(1));

        IMovable movable = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.Movable", obj.Object);

        Assert.Equal(movable.Speed, new Vector(1));
    }
}
