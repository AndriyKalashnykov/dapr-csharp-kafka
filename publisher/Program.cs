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
                int retryCount = int.Parse(Environment.GetEnvironmentVariable("RETRY_COUNT") ?? "5");
                int retryDelay = int.Parse(Environment.GetEnvironmentVariable("RETRY_DELAY") ?? "2");

                for (int i = 0; i < retryCount; i++)
                {
                    try
                    {
                        // Try to connect to Dapr and Kafka
                        await client.PublishEventAsync(pubsubName, "healthcheck", new { Message = "Health check" });
                        Console.WriteLine("Message sent to Kafka!");
                        await Task.Delay(10000);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Connection attempt {i+1}/{retryCount} failed: {ex.Message}");
                        
                        Exception? innerException = ex.InnerException;
                        if (innerException != null)
                        {
                            Console.WriteLine($"Inner Exception: {innerException.Message}");
        
                            // Print stack trace for the inner exception
                            Console.WriteLine($"Inner Exception Stack Trace: {innerException.StackTrace}");
        
                            // Check for deeper nested exceptions
                            Exception? deeperException = innerException.InnerException;
                            if (deeperException != null)
                            {
                                Console.WriteLine($"Deeper Exception: {deeperException.Message}");
                            }
                        }
                        
                        if (i < retryCount - 1)
                        {
                            // Exponential backoff with jitter
                            int delayMs = retryDelay * 1000 * (int)Math.Pow(2, i);
                            Random random = new Random();
                            delayMs += random.Next(0, 1000);
                            Console.WriteLine($"Retrying in {delayMs/1000} seconds...");
                            await Task.Delay(delayMs);
                        }
                        else
                        {
                            Console.WriteLine("Max retries reached. Continuing anyway...");
                        }
                    }
                }
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