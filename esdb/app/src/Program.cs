
using System.Text.Json;
using EventStore.Client;

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

const string connectionString = "esdb://admin:changeit@node1:2113?tls=false&tlsVerifyCert=false";

const string streamName = "some-stream";
const string eventType = "TestEvent";

var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

Console.WriteLine("Hi!");

var t = Task.Run(async () =>
{
    while (!cancellationToken.IsCancellationRequested)
    {
        Console.WriteLine("Writing event");
        var evt = new TestEvent(Guid.NewGuid().ToString("N"), "I wrote my first event!");
        var eventData = new EventData(
            Uuid.NewUuid(),
            eventType,
            JsonSerializer.SerializeToUtf8Bytes(evt)
        );

        await client.AppendToStreamAsync(
            streamName,
            StreamState.Any,
            new[] { eventData },
            cancellationToken: cancellationToken
        );

        Console.WriteLine("Written event");
        await Task.Delay(1000);
    }
});

var result = client.ReadStreamAsync(
    Direction.Forwards,
    streamName,
    StreamPosition.Start,
    cancellationToken: cancellationToken
);


await client.SubscribeToStreamAsync(streamName, FromStream.Start, async (s, e, c) =>
{
    var te = JsonSerializer.Deserialize<TestEvent>(e.Event.Data.ToArray());
    Console.WriteLine(e.Event.EventNumber + "\t" + e.Event.EventType + "\t" + te);
    await Task.Yield();
});

await t;

Console.WriteLine("Hello, World!");
