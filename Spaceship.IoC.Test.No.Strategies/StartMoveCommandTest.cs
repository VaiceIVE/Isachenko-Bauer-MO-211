using Hwdtech;
using Spaceship__Server;
using Moq;

namespace Spaceship.IoC.Test.No.Strategies;

using System;


public class ContiniousMovement
{
    [Fact]
    public void MoveCommandContinious()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapters.IUObject.MoveCommand", (object[] args) => 
        {
            MovableAdapter adp = new MovableAdapter(args);
            return adp;
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
}
