using Dapr.Client;

namespace Samples.Client;

internal class Program
{
    protected static readonly string pubsubName = "mypubsub";
    protected static readonly string topicName = "event";
    private static readonly int delaySeconds = 5;
    private static bool keepRunning = true;

    private static async Task<int> Main(string[] args)
    {
        Console.CancelKeyPress += (sender, e) => {
            Console.WriteLine("Stopping publisher...");
            keepRunning = false;
            e.Cancel = true; // Prevent the process from terminating immediately
        };

        using var client = new DaprClientBuilder().Build();
        
        Console.WriteLine($"Publisher started. Publishing to {pubsubName}/{topicName} every {delaySeconds} seconds.");
        Console.WriteLine("Press Ctrl+C to exit.");
        
        while (keepRunning)
        {
            try
            {
                var eventData = new EventData(
                    Id: Guid.NewGuid().ToString(),
                    Data: DateTime.UtcNow
                );
                
                await client.PublishEventAsync(pubsubName, topicName, eventData);
                Console.WriteLine($"Published event: Id={eventData.Id}, Data={eventData.Data:o}");
                
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing event: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(1)); // Brief delay on error
            }
        }

        Console.WriteLine("Publisher stopped.");
        return 0;
    }
}

public record EventData(string Id, DateTime Data);