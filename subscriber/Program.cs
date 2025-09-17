using System.Text.Json.Serialization;
using Dapr;
using Dapr.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseCloudEvents();

// Allow Dapr to discover code-based subscriptions
app.MapSubscribeHandler();

// Simple health/home endpoint
app.MapGet("/", () => "Hello World!");

// Dapr will POST messages here. We accept EventData and print both fields.
app.MapPost("/event", (EventData evt) =>
{
    Console.WriteLine($"Id={evt.Id}, Data={evt.Data:o}");
    return Results.Ok();
}).WithTopic("mypubsub", "event");

app.Run();

public record EventData(
    [property: JsonPropertyName("Id")] string Id,
    [property: JsonPropertyName("Data")] DateTime Data
);