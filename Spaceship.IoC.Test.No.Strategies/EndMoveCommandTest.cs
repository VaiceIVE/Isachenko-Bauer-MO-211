using System;
using Spaceship__Server;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceship.IoC.Test.No.Strategies
{
    public class EndMoveCommandTest
    {
        [Fact]
        public void EndMoveCommand()
        {
            Queue<ICommand> _queue = new();

            Mock<ICommand> cmd = new();

            BridgeCommand bridge = new(cmd.Object);

            MacroCommand macro = new(_queue, new List<ICommand> { bridge });

            macro.Execute();

            Assert.Equal(2, _queue.Count);

            Assert.Equal(cmd.Object.GetType(), ((BridgeCommand)_queue.Peek()).internalCommand.GetType());

            _queue.Dequeue().Execute();

            _queue.Dequeue().Execute();

            Mock<IUObject> writ = new();

            writ.Setup(o => o.get_property("Command")).Returns(bridge);

            Mock<IUObject> obj = new();

            obj.Setup(o => o.get_property("Velocity")).Returns(1);

            writ.Setup(o => o.get_property("Object")).Returns(obj.Object);

            EndMoveCommand end = new(writ.Object);

            end.Execute();

            Assert.Equal("Spaceship__Server.EmptyCommand", ((BridgeCommand)_queue.Peek()).internalCommand.GetType().ToString());
        }
        [Fact]
        public void NothingTest()
        {
            EmptyCommand cmd = new();
            cmd.Execute();
        }
    }
}