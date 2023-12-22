
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using EventStore.Client;

var testDebug = new TestEvent("aasdasd", "sdfsdfsdf sdfsdf");

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

//const string connectionString = "esdb://admin:changeit@node1:2113?tls=false&tlsVerifyCert=false";
const string connectionString = "esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";

const string streamPrefix = "user-";

var settings = EventStoreClientSettings.Create(connectionString);
var client = new EventStoreClient(settings);

Console.WriteLine("Hi!");



var t = Task.Run(async () =>
{
    while (!cancellationToken.IsCancellationRequested)
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey();
            Console.WriteLine("Enter user Id");
            var id = Console.ReadLine();
            var streamName = streamPrefix + id;

            var eventData = key.Key switch
            {
            ConsoleKey.A => new EventData(
                                    Uuid.NewUuid(),
                                    "UserRegistered",
                                    JsonSerializer.SerializeToUtf8Bytes(
                                        new TestEvent(Guid.NewGuid().ToString("N"), "I wrote my first event!")
                                )),
            ConsoleKey.B => new EventData(
                                    Uuid.NewUuid(),
                                    "UserNameSet",
                                    JsonSerializer.SerializeToUtf8Bytes(
                                        new FirstNameSetEvent("SomeNameSet")
                                )),
                    
            _ => null
            };

            if(eventData!=null)
                await client.AppendToStreamAsync(
                    streamName,
                    StreamState.Any,
                    new[] { eventData },
                    cancellationToken: cancellationToken
                );

            Console.WriteLine("Written event");
        }
        else
        {
            await Task.Delay(50);
        }
    }
});

var readStreamName = "$ce-user";


await client.SubscribeToStreamAsync(
    readStreamName, FromStream.Start, async (s, e, c) =>
{
    var evt = e.Event;
    var d = evt.Data.ToArray();
    var str = Encoding.UTF8.GetString(d);
    var te = JsonSerializer.Deserialize<TestEvent>(d);
    Console.WriteLine(evt.EventNumber + "\t" + evt.EventType + "\t" + te);
    await Task.Yield();
}, resolveLinkTos: true);

await t;

Console.WriteLine("Hello, World!");
