using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaceship__Server
{
    public interface IInjectable
    {
        public void Inject(ICommand other);
    }
    public class BridgeCommand: ICommand, IInjectable
    {
        public ICommand internalCommand 
        { 
            get; 
            set; 
        }
        public BridgeCommand(ICommand cmd)
        {
            internalCommand = cmd;
        }
        public void Inject(ICommand other)
        {
            internalCommand = other;
        }

        public void Execute()
        {
            internalCommand.Execute();
        }

    }
    public interface MoveCommandEndable : ICommand
    {
        BridgeCommand EndCommand
        {
            get;
        }
        IUObject uObject
        {
            get;
        }
    }

    public class EndMoveCommand : MoveCommandEndable
    {
        public BridgeCommand EndCommand
        {
            get;
        }
        public IUObject uObject
        {
            get;
        }
        public EndMoveCommand(IUObject writ)
        { 
            uObject = (IUObject)writ.get_property("Object");
            EndCommand = (BridgeCommand)writ.get_property("Command");
        }
        public void Execute()
        {
            uObject.set_property("Velocity", null);
            EndCommand.Inject(new EmptyCommand());
        }
    }

}
