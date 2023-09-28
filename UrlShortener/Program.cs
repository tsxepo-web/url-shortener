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

var mongodbSettings = new UrlShortenerDatabaseSettings
{
    CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING"),
    DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME"),
    COLLECTION_NAME = Environment.GetEnvironmentVariable("COLLECTION_NAME")

};

//var mongodbSettings = builder.Configuration.GetSection("Shorteners").Get<UrlShortenerDatabaseSettings>()!;
var mongoClient = new MongoClient(mongodbSettings.CONNECTION_STRING);
var mongoDatabase = mongoClient.GetDatabase(mongodbSettings.DATABASE_NAME);
builder.Services.AddSingleton<IMongoCollection<UrlMapping>>(ProviderAliasAttribute =>
    mongoDatabase.GetCollection<UrlMapping>(mongodbSettings.COLLECTION_NAME));

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
