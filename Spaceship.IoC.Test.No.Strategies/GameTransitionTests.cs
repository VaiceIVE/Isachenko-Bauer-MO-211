using Hwdtech;
using System;
using Spaceship__Server;
using System.Collections.Concurrent;
using System.Diagnostics;
using Moq;

namespace Spaceship.IoC.Test.No.Strategies;

public class GameTransitionTests
{
    [Fact]
    public void SerializationTest()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();
        bool exceptionWasHandled = false;
        bool commandExecuted = false;

        Dictionary<string, object> gameObjects = new();

        Queue<Spaceship__Server.ICommand> queue = new();

        TimeSpan ts = new TimeSpan(0, 0, 0, 0, 100);

        Mock<IUObject> _obj = new();

        Mock<Spaceship__Server.ICommand> mcmd = new();

        mcmd.Setup(c => c.Execute()).Callback(() => {commandExecuted = true;});

        Spaceship__Server.ICommand cmd = mcmd.Object;


        gameObjects.Add("obj123",new TwoPhaseObject());

        Spaceship__Server.ICommand command = new ActionCommand(() => {});

        queue.Enqueue(command);

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Objects.GetAll", (object[] args) => 
        {
            return gameObjects;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Queue.Get", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Get.Timespan", (object[] args) => 
        {
            return (object)ts;
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Timespan", (object[] args) => 
        {
            return (object) ts;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Queue", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.HandleCommand", (object[] args) => 
        {
            Hwdtech.IoC.Resolve<Queue<Spaceship__Server.ICommand>>("Game.Current.Queue").TryDequeue(out cmd!);
            return cmd;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Exception.Source", (object[] args) => 
        {
        
            Exception ex = (Exception)args[0];
            var a = (new StackTrace(ex).GetFrame(0)!.GetMethod()!.ReflectedType)!.FullName;
            return a;
            
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetProps", (object[] args) => 
        {
            Dictionary<string, object> dict = new(){{"key", "string value"}};    
            return dict;        
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetAction", (object[] args) => 
        {
            return "{}";      
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObject", (object[] args) => 
        {
            string result_string = "";
            object obj = args[0];

            result_string += obj.ToString();

            result_string += Hwdtech.IoC.Resolve<string>("StringifyObjectProps", obj);

            return result_string;      
        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObjectProps", (object[] args) => 
        {
            string result_string = "{key=string value, key2=int 123}";
            return result_string;      
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object [] args) => 
        {
            var err = args[0];
            var command = args[1];
            Dictionary<string, Spaceship__Server.ICommand> subtree = new();

            Mock<Spaceship__Server.ICommand> defaultStrategy = new();

            defaultStrategy.Setup(s => s.Execute()).Callback(() => {throw (System.Exception) err;});
            
            var errtype = err.GetType();

            Mock<Spaceship__Server.ICommand> mcmd = new();

            Spaceship__Server.ICommand cmd = new ActionCommand(() => {});

            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = 
            Hwdtech.IoC.Resolve<Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>>>("Handler.Tree");

            if(tree.TryGetValue(command.ToString()!, out subtree!))
            {
                if(subtree.TryGetValue(errtype.ToString(), out cmd!))
                {
                    return cmd;
                }
            }

            return defaultStrategy.Object;

        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Tree", (object [] args) =>
        {
            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = new();
            
            Spaceship__Server.ICommand HandleStrategy = new ActionCommand(() => { exceptionWasHandled = true;});

            Spaceship__Server.ICommand OtherHandleStrategy = new ActionCommand(() => {});

            Dictionary<string, Spaceship__Server.ICommand> Exceptions = new(){{"System.Exception", HandleStrategy}, {"System.ArgumentException", OtherHandleStrategy}};

            tree = new(){{"Spaceship__Server.ExceptionThrower", Exceptions}, {"Spaceship__Server.MoveCommand", Exceptions}};

            return tree;

        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "SerializeCommand", (object[] args) => 
        {
            Spaceship__Server.ICommand cmd = (Spaceship__Server.ICommand) args[0];

            string return_string = "Command ";

            if(cmd is MoveCommand)
            {
                return_string += "type=move, ";
                foreach(var prop in Hwdtech.IoC.Resolve<Dictionary<string, object>>("Command.GetProps", cmd)){
                    return_string += prop.Key + " : " + prop.Value.ToString();
                }
            }
            if(cmd is ActionCommand){
                return_string += "type=action, ";
                return_string += "action=" + Hwdtech.IoC.Resolve<string>("Command.GetAction", cmd);
            }

            return return_string;
        }).Execute();

        Spaceship__Server.ICommand gameCommand = new GameCommand(scope);

        string serializedGame = GameSerializer.Serialize("1");

        Assert.Equal("obj123 : Spaceship__Server.TwoPhaseObject{key=string value, key2=int 123}; | Command type=action, action={} | 00:00:00.1000000", serializedGame);

    }

    [Fact]
    public void DeserializationTest()
    {

        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"));

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Dictionary<string, object> deserializedGameObjects = new();

        Queue<Spaceship__Server.ICommand> deserializedQueue = new();

        TimeSpan dts = new();

        bool exceptionWasHandled = false;
        bool commandExecuted = false;

        Dictionary<string, object> gameObjects = new();

        Queue<Spaceship__Server.ICommand> queue = new();

        TimeSpan ts = new TimeSpan(0, 0, 0, 0, 100);


        Mock<Spaceship__Server.ICommand> mcmd = new();

        mcmd.Setup(c => c.Execute()).Callback(() => {commandExecuted = true;});

        Spaceship__Server.ICommand cmd = mcmd.Object;

        IUObject _obj = new TwoPhaseObject();

        gameObjects.Add("obj123", _obj);

        Spaceship__Server.ICommand command = new ActionCommand(() => {});

        queue.Enqueue(command);

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeValue", (object[] args) => {
            string serializedString = (string) args[0];
            return (object) _obj;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeCommand", (object[] args) => {
            string serializedCommand = (string) args[0];
            return (object) command;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeTimespan", (object[] args) => {
            string serializedTimespan = (string) args[0];
            return (object) ts;
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateNewGameCommand", (object[] args) => {
            deserializedGameObjects = (Dictionary<string, object>) args[0];
            deserializedQueue = (Queue<Spaceship__Server.ICommand>) args[1];
            dts = (TimeSpan)args[2];
            return (object)new ActionCommand(() => {}); 
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Objects.GetAll", (object[] args) => 
        {
            return gameObjects;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Queue.Get", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Get.Timespan", (object[] args) => 
        {
            return (object)ts;
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Timespan", (object[] args) => 
        {
            return (object) ts;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.Queue", (object[] args) => 
        {
            return queue;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "Game.Current.HandleCommand", (object[] args) => 
        {
            Hwdtech.IoC.Resolve<Queue<Spaceship__Server.ICommand>>("Game.Current.Queue").TryDequeue(out cmd!);
            return cmd;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Exception.Source", (object[] args) => 
        {
        
            Exception ex = (Exception)args[0];
            var a = (new StackTrace(ex).GetFrame(0)!.GetMethod()!.ReflectedType)!.FullName;
            return a;
            
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetProps", (object[] args) => 
        {
            Dictionary<string, object> dict = new(){{"key", "string value"}};    
            return dict;        
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetAction", (object[] args) => 
        {
            return "{}";      
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObject", (object[] args) => 
        {
            string result_string = "";
            object obj = args[0];

            result_string += obj.ToString();

            result_string += Hwdtech.IoC.Resolve<string>("StringifyObjectProps", obj);

            return result_string;      
        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObjectProps", (object[] args) => 
        {
            string result_string = "{key=string value, key2=int 123}";
            return result_string;      
        }).Execute();


        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HandleException", (object [] args) => 
        {
            var err = args[0];
            var command = args[1];
            Dictionary<string, Spaceship__Server.ICommand> subtree = new();

            Mock<Spaceship__Server.ICommand> defaultStrategy = new();

            defaultStrategy.Setup(s => s.Execute()).Callback(() => {throw (System.Exception) err;});
            
            var errtype = err.GetType();

            Mock<Spaceship__Server.ICommand> mcmd = new();

            Spaceship__Server.ICommand cmd = new ActionCommand(() => {});

            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = 
            Hwdtech.IoC.Resolve<Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>>>("Handler.Tree");

            if(tree.TryGetValue(command.ToString()!, out subtree!))
            {
                if(subtree.TryGetValue(errtype.ToString(), out cmd!))
                {
                    return cmd;
                }
            }

            return defaultStrategy.Object;

        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Tree", (object [] args) =>
        {
            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = new();
            
            Spaceship__Server.ICommand HandleStrategy = new ActionCommand(() => { exceptionWasHandled = true;});

            Spaceship__Server.ICommand OtherHandleStrategy = new ActionCommand(() => {});

            Dictionary<string, Spaceship__Server.ICommand> Exceptions = new(){{"System.Exception", HandleStrategy}, {"System.ArgumentException", OtherHandleStrategy}};

            tree = new(){{"Spaceship__Server.ExceptionThrower", Exceptions}, {"Spaceship__Server.MoveCommand", Exceptions}};

            return tree;

        }).Execute();

         Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register" , "SerializeCommand", (object[] args) => 
        {
            Spaceship__Server.ICommand cmd = (Spaceship__Server.ICommand) args[0];

            string return_string = "Command ";

            if(cmd is MoveCommand)
            {
                return_string += "type=move, ";
                foreach(var prop in Hwdtech.IoC.Resolve<Dictionary<string, object>>("Command.GetProps", cmd)){
                    return_string += prop.Key + " : " + prop.Value.ToString();
                }
            }
            if(cmd is ActionCommand){
                return_string += "type=action, ";
                return_string += "action=" + Hwdtech.IoC.Resolve<string>("Command.GetAction", cmd);
            }

            return return_string;
        }).Execute();

        Spaceship__Server.ICommand gameCommand = new GameCommand(scope);

        string serializedGame = GameSerializer.Serialize("1");

        Spaceship__Server.ICommand deserializedGameCommand = GameDeserializer.Deserialize(serializedGame);

        Assert.Equal("obj123 : Spaceship__Server.TwoPhaseObject{key=string value, key2=int 123}; | Command type=action, action={} | 00:00:00.1000000", serializedGame);
        Assert.Equal(queue, deserializedQueue);
        Assert.Equal(gameObjects, deserializedGameObjects);
        Assert.Equal(ts, dts);
    }
}
