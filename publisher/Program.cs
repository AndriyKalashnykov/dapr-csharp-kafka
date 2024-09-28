using Dapr.Client;

namespace Samples.Client;

internal class Program
{
    protected static readonly string pubsubName = "pubsub";

    private static async Task<int> Main(string[] args)
    {
        using var client = new DaprClientBuilder().Build();

        var eventData = new { Id = "17" };
        await client.PublishEventAsync("mypubsub", "deposit", eventData);
        Console.WriteLine("Published deposit event!");

        return 0;
    }
}