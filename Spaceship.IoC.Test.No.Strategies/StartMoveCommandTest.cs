using Hwdtech;
using Spaceship__Server;
using System;
using Moq;

namespace Spaceship.IoC.Test.No.Strategies;

using System;

public class ContiniousMovement
{
    [Fact]
    public void MoveCommandContinious()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.IMovable", (object[] args) => 
        {
            return new MovableAdapter(args);
        }).Execute();

        Mock<IUObject> order = new();

        Mock<IUObject> _obj = new();

        Queue<Spaceship__Server.ICommand> _queue = new();

        _obj.Setup(o => o.get_property("Velocity")).Returns((object) new Vector(1, 1));

        _obj.Setup(o => o.get_property("Position")).Returns((object) new Vector(0, 0));

        order.Setup(o => o.get_property("Object")).Returns((object) _obj.Object);

        order.Setup(o => o.get_property("Queue")).Returns((object) _queue);

        order.Setup(o => o.get_property("Velocity")).Returns((object) new Vector(5, 5) );

        StartMoveCommand cmd = new(order.Object);
        
        cmd.Execute();

        Assert.Equal(2, _queue.Count);
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
    [Fact]
    public void MacroExecutionSuccess()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.Movable", (object[] args) => 
        {
            MovableAdapter adp = new MovableAdapter(args);
            return adp;
        }).Execute();

        Queue<Spaceship__Server.ICommand> _queue = new();

        Mock<IUObject> obj = new();

        obj.Setup(o => o.get_property("Position")).Returns(new Vector(2));

        obj.Setup(o => o.get_property("Velocity")).Returns(new Vector(1));

        IMovable movable = Hwdtech.IoC.Resolve<IMovable>("Adapters.IUObject.Movable", obj.Object);

        new MacroCommand(_queue, new List<Spaceship__Server.ICommand>(){new MoveCommand(movable)}).Execute();

        Assert.Equal(2, _queue.Count);
    }
}
