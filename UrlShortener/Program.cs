using Microsoft.Extensions.Configuration;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using MongoDB.Driver;
using SharedModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
var collectionName = Environment.GetEnvironmentVariable("COLLECTION_NAME");

//var mongodbSettings = builder.Configuration.GetSection("Shorteners").Get<UrlShortenerDatabaseSettings>()!;
var mongoClient = new MongoClient(connectionString);
var mongoDatabase = mongoClient.GetDatabase(databaseName);
builder.Services.AddSingleton<IMongoCollection<UrlMapping>>(ProviderAliasAttribute => mongoDatabase.GetCollection<UrlMapping>(collectionName));

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
