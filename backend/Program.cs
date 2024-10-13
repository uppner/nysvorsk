using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<CosmosClient>(_ =>
{
  return new CosmosClient(builder.Configuration["CosmosConnectionString"]);
});

var app = builder.Build();


var conn = app.Configuration["CosmosConnectionString"];

app.UseSwagger();
app.UseSwaggerUI();

app.UsePathBase("/api");

app.MapGet("/", () => "OK");

app.MapGet("/words", async ([FromServices] CosmosClient client) =>
{
  var wordContainer = client.GetContainer("nysvorsk", "words");
  var wordIterator = wordContainer.GetItemQueryIterator<Word>("select * from c");
  var result = await wordIterator.ReadNextAsync();
  var words = result.ToList();
  return Results.Ok(words);
});

app.Run();

public record Word(string name, string status, string text);