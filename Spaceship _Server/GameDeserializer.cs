namespace Spaceship__Server;
using Hwdtech;
using System;
using System.Diagnostics;
using System.Collections.Generic;
public class GameDeserializer{

    public static Spaceship__Server.ICommand Deserialize(string serializedGame){
        string[] serializedData = serializedGame.Split('|');

        Dictionary<string, object> gameObjects = new();
        Queue<Spaceship__Server.ICommand> gameQueue = new();
//"obj123 : Spaceship__Server.TwoPhaseObject{key=string value, key2=int 123}; | Command type=action, action={} | 00:00:00.1000000"
        foreach(string propertyData in serializedData[0].Split(';')){
            if (propertyData.Contains(':'))
            {
                string key = propertyData.Split(" : ")[0];
                string stringValue = propertyData.Split(" : ")[1];

                object objectValue = Hwdtech.IoC.Resolve<object>("DeserializeValue", stringValue);

                gameObjects[key] = objectValue;
            }
            
        }

        foreach(string commandData in serializedData[1].Split(',')){
            if(commandData.Contains("type"))
            {
                Spaceship__Server.ICommand deserializedCommand = Hwdtech.IoC.Resolve<ICommand>("DeserializeCommand", commandData);

                gameQueue.Enqueue(deserializedCommand);            
            }
            
        }

        TimeSpan timespan = Hwdtech.IoC.Resolve<TimeSpan>("DeserializeTimespan", serializedData[2]);

        Spaceship__Server.ICommand newGameCommand = Hwdtech.IoC.Resolve<Spaceship__Server.ICommand>("CreateNewGameCommand", gameObjects, gameQueue, timespan);

        return newGameCommand;
    }
}