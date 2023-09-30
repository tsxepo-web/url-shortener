using Microsoft.Extensions.Configuration;
using dotenv.net;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using MongoDB.Driver;
using SharedModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

DotEnv.Load();
var CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
var COLLECTION_NAME = Environment.GetEnvironmentVariable("COLLECTION_NAME");

var mongoClient = new MongoClient(CONNECTION_STRING);
var mongoDatabase = mongoClient.GetDatabase(DATABASE_NAME);
builder.Services.AddSingleton<IMongoCollection<UrlMapping>>(
    ProviderAliasAttribute =>
        mongoDatabase.GetCollection<UrlMapping>(COLLECTION_NAME));


builder.Services.AddTransient<IUrlShortener, UrlShortenerService>();
builder.Services.AddTransient<IUrlMappingRepository, UrlMappingRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapFallback(handler: async (IUrlMappingRepository _repository) =>
{
    var urlMatch = await _repository.FindByShortUrlAsync();
    return Results.Redirect(urlMatch.Url);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
