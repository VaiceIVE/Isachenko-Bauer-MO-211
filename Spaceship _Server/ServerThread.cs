using System;
using System.Threading;
using Hwdtech;
namespace Spaceship__Server;

public class MyThread
{
    public Thread thread;
    public IReciver receiver;
    public IReciver orderReciever;
    public bool stop = false;
    public Action strategy;

    internal void Stop() => stop = true;

    internal void HandleCommand()
    {
        if(!receiver.isEmpty())
        {
            var cmd = receiver.Receive();

            cmd.Execute();
        }
        
        if(!orderReciever.isEmpty())
        {
            var order = orderReciever.Receive();

            order.Execute();
        }
    }
    public MyThread(IReciver queue, IReciver orderQueue)
    {
        this.orderReciever = orderQueue;
        this.receiver = queue;
        strategy = () =>
        {
            HandleCommand();
        };

        thread = new Thread(() =>
        {
            while (!stop) strategy();
        });
    }
    internal void UpdateBehaviour(Action newBehaviour)
    {
        strategy = newBehaviour;

    }
    public void Start()
    {
        thread.Start();
    }
}
