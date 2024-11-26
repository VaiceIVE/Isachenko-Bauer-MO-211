namespace Spaceship__Server;
using System.Collections.Generic;

public interface IRouter{
    public bool Route(string id, Dictionary<string, object> payload);
}