using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseCloudEvents();

app.MapGet("/", () => "Hello World!");

app.MapPost("/event", (EventData deposit) =>
{
    Console.WriteLine(deposit.Id);
    return Results.Ok();
});

app.Run();

public record EventData([property: JsonPropertyName("Id")] string Id);