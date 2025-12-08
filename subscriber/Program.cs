using System.Text.Json.Serialization;
using Dapr;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Dapr configurations
app.UseCloudEvents();

app.MapSubscribeHandler();

app.MapPost("/event", [Topic("mypubsub", "event")] (ILogger<Program> logger, Message msg) => {
    Console.WriteLine($"{msg.Id}: {msg.Data}");
    return Results.Ok();
});


// Alternative endpoint - commented out to avoid duplicate subscriptions
/*
app.MapPost("/event", async (HttpRequest request, EventData evt) =>
{
    request.EnableBuffering();
    using var reader = new StreamReader(request.Body, leaveOpen: true);
    var body = await reader.ReadToEndAsync();
    request.Body.Position = 0;

    Console.WriteLine(body);
    Console.WriteLine($"Id={evt.Id}, Data={evt.Data:o}");
    return Results.Ok();
}).WithTopic("mypubsub", "event");
*/

app.Run();

public record Message(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("data")] DateTime Data
);