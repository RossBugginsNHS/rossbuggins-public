
using System.Text.Json;
using EventStore.Client;

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

const string connectionString = "esdb://admin:changeit@node1:2113?tls=false&tlsVerifyCert=false";

const string streamPrefix = "users-";
const string eventType1 = "TestEvent";
const string eventType2 = "AnotherTestEvent";
const string streamsReadPrefix = "$ce-users";

var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

Console.WriteLine("Hi!");

var t = Task.Run(async () =>
{
    while (!cancellationToken.IsCancellationRequested)
    {
        var userNumber = Random.Shared.Next(1, 100);

        var choice = Random.Shared.Next(0,2);

        Console.WriteLine("Writing event");

        
        var eventData = 
            choice switch
            {
                0 =>    new EventData(
                            Uuid.NewUuid(),
                            eventType1,
                            JsonSerializer.SerializeToUtf8Bytes(
                                new TestEvent(Guid.NewGuid().ToString("N"), "I wrote my first event!")
                            )),
                1 =>    new EventData(
                            Uuid.NewUuid(),
                            eventType2,
                            JsonSerializer.SerializeToUtf8Bytes(
                                new AnotherTestEvent(Guid.NewGuid().ToString("N"), "I some other event!")
                            )),
                _ => null
            };

        if(eventData!=null)
        await client.AppendToStreamAsync(
            streamPrefix + userNumber,
            StreamState.Any,
            new[] { eventData },
            cancellationToken: cancellationToken
        );

        Console.WriteLine("Written event");
        await Task.Delay(1000);
    }
});



await client.SubscribeToStreamAsync(streamsReadPrefix, FromStream.Start, async (s, e, c) =>
{
    var te = JsonSerializer.Deserialize<TestEvent>(e.Event.Data.ToArray());
    Console.WriteLine(e.Event.EventNumber + "\t" + e.Event.EventType + "\t" + te);
    await Task.Yield();
}, resolveLinkTos: true);

await t;

Console.WriteLine("Hello, World!");
