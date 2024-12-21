using System.Threading.Tasks;
using Grpc.Net.Client;
using MessageListener;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Npgsql;

using var channel = GrpcChannel.ForAddress("http://localhost:5103");
var client = new Greeter.GreeterClient(channel);

var messages = new List<string>();
var connString = "Host=localhost;Username=postgres;Password=postgres;Database=transactional_outbox";
await using var conn = new NpgsqlConnection(connString);
await conn.OpenAsync();
await using (var cmd = new NpgsqlCommand("SELECT message FROM messages WHERE is_sent=false", conn))
await using (var reader = await cmd.ExecuteReaderAsync())
{
while (await reader.ReadAsync())
    messages.Add(reader.GetString(0));
}
foreach(string mes in messages){
    Dictionary<string, string> dict = new();
    foreach(string prop in mes.Split(';')){
        dict.Add(prop.Split('=')[0], prop.Split('=')[1]);
    }
    try{
        var props = new MapField<string, string>();
        props.Add(dict);
        var gRPCMess = new gRPCMessage();
        gRPCMess.Props.Add(props);
        var reply = client.SayHello(gRPCMess);
        Console.WriteLine("Greeting: " + reply.Status);
        await using (var cmd = new NpgsqlCommand("UPDATE messages SET is_sent=true WHERE \"message\"=(@p)", conn))
        {
            Console.WriteLine(mes);
            cmd.Parameters.AddWithValue("p", mes);
            await cmd.ExecuteNonQueryAsync();
        }
    }
    catch{
        continue;
    }
    
}



