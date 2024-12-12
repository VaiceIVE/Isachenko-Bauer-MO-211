namespace Spaceship__Server;
using Hwdtech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
public class GameSerializer{

    public static string Serialize(string gameId){

        string serializedGame = "";

        Dictionary<string, object> gameObjects = Hwdtech.IoC.Resolve<Dictionary<string, object>>("Game.Objects.GetAll", gameId);
        Queue<Spaceship__Server.ICommand> gameQueue = Hwdtech.IoC.Resolve<Queue<Spaceship__Server.ICommand>>("Game.Queue.Get", gameId);
        TimeSpan timespan = Hwdtech.IoC.Resolve<TimeSpan>("Game.Get.Timespan", gameId);

        foreach(KeyValuePair<string, object> entry in gameObjects)
        {
            serializedGame += entry.Key + " : " + Hwdtech.IoC.Resolve<string>("StringifyObject", entry.Value) + ";";
        }

        serializedGame += " | ";

        foreach(ICommand cmd in gameQueue.ToArray()){
            serializedGame += Hwdtech.IoC.Resolve<string>("SerializeCommand", cmd);
        }

        serializedGame += " | ";

        serializedGame += timespan.ToString();

        return serializedGame;
    }
}
