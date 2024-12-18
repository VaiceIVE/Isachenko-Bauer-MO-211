﻿using System.Threading.Tasks;
using Grpc.Net.Client;
using System;

namespace GrpcGreeterClient;

public class GrpcClient
{
    public static void Call(string ip, string message)
    {
        using var channel = GrpcChannel.ForAddress(ip);
        var client = new Greeter.GreeterClient(channel);
        var response = client.AcceptGame(new serializedGameMessage{SerializedGame = message});
        Console.WriteLine(response);
    }
}
