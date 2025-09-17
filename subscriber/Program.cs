using System.Text.Json.Serialization;
using System.IO;
using Dapr;
using Dapr.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseCloudEvents();

// Allow Dapr to discover code-based subscriptions
app.MapSubscribeHandler();

// Simple health/home endpoint
app.MapGet("/", () => "Hello World!");

// Dapr will POST messages here. We read and print the raw body.
app.MapPost("/event", async (HttpRequest request) =>
{
    request.EnableBuffering();
    using var reader = new StreamReader(request.Body, leaveOpen: true);
    var body = await reader.ReadToEndAsync();
    request.Body.Position = 0;

    Console.WriteLine(body);
    return Results.Ok();
}).WithTopic("mypubsub", "event");

app.Run();

public record EventData([property: JsonPropertyName("Id")] string Id);