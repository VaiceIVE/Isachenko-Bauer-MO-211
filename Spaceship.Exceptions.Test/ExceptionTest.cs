using Hwdtech;
using Spaceship__Server;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Moq;
using System.Diagnostics;

namespace Spaceship.Exceptions.Test;

public class ExceptionTests
{
    [Fact]
    public void ExHandlerSuccessful()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Exception.Source", (object[] args) => 
        {
            Exception ex = (Exception)args[0];
            var a = (new StackTrace(ex).GetFrame(0)!.GetMethod()!.ReflectedType)!.FullName;
            return a;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Command.Priority", (object [] args) => 
        {
            var err = args[0].GetType();
            Console.WriteLine(err);
            var command = args[1];
            Console.WriteLine(command);
            Dictionary<string, Spaceship__Server.ICommand> subtree = new();

            Mock<Spaceship__Server.ICommand> defaultStrategy = new();

            Mock<Spaceship__Server.ICommand> mcmd = new();

            var cmd = mcmd.Object;

            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = 
            IoC.Resolve<Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>>>("Handler.Tree.Command.Priority");

            if(tree.TryGetValue(command.ToString()!, out subtree!))
            {
                if(subtree.TryGetValue(err.ToString(), out cmd))
                {
                    return cmd;
                }
            }
                return defaultStrategy.Object;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Exception.Priority", (object [] args) => 
        {
            var err = args[0].GetType();
            Console.WriteLine(err);
            var command = args[1];
            Console.WriteLine(command);
            Dictionary<string, Spaceship__Server.ICommand> subtree = new();

            Mock<Spaceship__Server.ICommand> defaultStrategy = new();

            Mock<Spaceship__Server.ICommand> mcmd = new();

            var cmd = mcmd.Object;

            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = 
            IoC.Resolve<Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>>>("Handler.Tree.Exception.Priority");
            
            if(tree.TryGetValue(err.ToString(), out subtree!))
            {
                if(subtree.TryGetValue(command.ToString()!, out cmd))
                {
                    return cmd;
                }
            }
                return defaultStrategy.Object;
        }).Execute();
        

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Tree.Command.Priority", (object [] args) =>
        {
            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = new();
            
            Mock<Spaceship__Server.ICommand> HandleStrategy = new();

            Mock<Spaceship__Server.ICommand> OtherHandleStrategy = new();

            Dictionary<string, Spaceship__Server.ICommand> Exceptions = new(){{"System.Exception", HandleStrategy.Object}, {"System.ArgumentException", OtherHandleStrategy.Object}};

            tree = new(){{"Spaceship__Server.ExceptionThrower", Exceptions}, {"Spaceship__Server.MoveCommand", Exceptions}};

            return tree;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Tree.Exception.Priority", (object [] args) =>
        {
            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = new();
            
            Mock<Spaceship__Server.ICommand> HandleStrategy = new();

            Mock<Spaceship__Server.ICommand> OtherHandleStrategy = new();

            Dictionary<string, Spaceship__Server.ICommand> Exceptions = new(){{"Spaceship__Server.ExceptionThrower", HandleStrategy.Object}, {"Spaceship__Server.MoveCommand", OtherHandleStrategy.Object}};

            tree = new(){{"System.Exception", Exceptions}, {"System.ArgumentException", Exceptions}};

            return tree;
        }).Execute();

        try
        {
            ExceptionThrower thr = new();
            thr.ThrowEx();
        }catch(Exception e)
        {
            IoC.Resolve<Spaceship__Server.ICommand>("ExceptionHandler.Command.Priority", e, IoC.Resolve<string>("Get.Exception.Source", e)).Execute();
        }

    }

    [Fact]
    public void ExHandlerNotFound()
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Exception.Source", (object[] args) => 
        {
            Exception ex = (Exception)args[0];
            var a = (new StackTrace(ex).GetFrame(0)!.GetMethod()!.ReflectedType)!.FullName;
            return a;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Command.Priority", (object [] args) => 
        {
            var err = args[0].GetType();
            Console.WriteLine(err);
            var command = args[1];
            Console.WriteLine(command);
            Dictionary<string, Spaceship__Server.ICommand> subtree = new();

            Mock<Spaceship__Server.ICommand> defaultStrategy = new();

            Mock<Spaceship__Server.ICommand> mcmd = new();

            var cmd = mcmd.Object;

            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = 
            IoC.Resolve<Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>>>("Handler.Tree.Command.Priority");

            if(tree.TryGetValue(command.ToString()!, out subtree!))
            {
                if(subtree.TryGetValue(err.ToString(), out cmd))
                {
                    return cmd;
                }
            }
                return defaultStrategy.Object;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Exception.Priority", (object [] args) => 
        {
            var err = args[0].GetType();
            Console.WriteLine(err);
            var command = args[1];
            Console.WriteLine(command);
            Dictionary<string, Spaceship__Server.ICommand> subtree = new();

            Mock<Spaceship__Server.ICommand> defaultStrategy = new();

            Mock<Spaceship__Server.ICommand> mcmd = new();

            var cmd = mcmd.Object;

            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = 
            IoC.Resolve<Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>>>("Handler.Tree.Exception.Priority");
            
            if(tree.TryGetValue(err.ToString(), out subtree!))
            {
                if(subtree.TryGetValue(command.ToString()!, out cmd))
                {
                    return cmd;
                }
            }
                return defaultStrategy.Object;
        }).Execute();
        

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Tree.Command.Priority", (object [] args) =>
        {
            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = new();
            
            Mock<Spaceship__Server.ICommand> HandleStrategy = new();

            Mock<Spaceship__Server.ICommand> OtherHandleStrategy = new();

            Dictionary<string, Spaceship__Server.ICommand> Exceptions = new(){{"System.Exception", HandleStrategy.Object}, {"System.ArgumentException", OtherHandleStrategy.Object}};

            tree = new(){{"Spaceship__Server.ExceptionThrower", Exceptions}, {"Spaceship__Server.MoveCommand", Exceptions}};

            return tree;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Tree.Exception.Priority", (object [] args) =>
        {
            Dictionary<string, Dictionary<string, Spaceship__Server.ICommand>> tree = new();
            
            Mock<Spaceship__Server.ICommand> HandleStrategy = new();

            Mock<Spaceship__Server.ICommand> OtherHandleStrategy = new();

            Dictionary<string, Spaceship__Server.ICommand> Exceptions = new(){{"Spaceship__Server.ExceptionThrower", HandleStrategy.Object}, {"Spaceship__Server.MoveCommand", OtherHandleStrategy.Object}};

            tree = new(){{"System.Exception", Exceptions}, {"System.ArgumentException", Exceptions}};

            return tree;
        }).Execute();
        try
        {
            Mock<Spaceship__Server.ICommand> cmd = new();

            cmd.Setup(c => c.Execute()).Throws<Exception>();
        }
        catch(Exception e)
        {
            Hwdtech.IoC.Resolve<object>("ExceptionHandler.Exception.Priority", e, IoC.Resolve<string>("Get.Exception.Source", e));
        }
    }
}
