namespace Spaceship__Server;
using System.Collections.Generic;

public class DesserializeSendCommand: ICommand{

    private Dictionary<string, object> _props;

    public DesserializeSendCommand(Dictionary<string, object> properties)
    {
        this._props = properties;
    }
    public void Execute()
    {   
        string MessageType = (string) _props["type"];

        Spaceship__Server.ICommand cmd = Hwdtech.IoC.Resolve<Spaceship__Server.ICommand>("Create " + MessageType + " by Message", _props);

        ((Spaceship__Server.ICommand)Hwdtech.IoC.Resolve<Spaceship__Server.ICommand>("Send Command", cmd)).Execute();
    }
}
