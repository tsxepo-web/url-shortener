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
var CONNECTION_STRING = "mongodb://url-shortener-db:tB7kQlucl34G8prxrOMtgnDuxSjQTGsKAbsdY4ghiwgBKzCf2BSj9t7SAQZ3jnGtJzZVvoUYI2SsACDbwnEeYg==@url-shortener-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@url-shortener-db@";


DotEnv.Load();
//var CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");
//var DATABASE_NAME = Environment.GetEnvironmentVariable("DATABASE_NAME");
//var COLLECTION_NAME = Environment.GetEnvironmentVariable("COLLECTION_NAME");

var mongoClient = new MongoClient(CONNECTION_STRING);
var mongoDatabase = mongoClient.GetDatabase("UrlShortener");
builder.Services.AddSingleton<IMongoCollection<UrlMapping>>(
    ProviderAliasAttribute =>
        mongoDatabase.GetCollection<UrlMapping>("shorteners"));


builder.Services.AddTransient<IUrlShortener, UrlShortenerService>();
builder.Services.AddTransient<IUrlMappingRepository, UrlMappingRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapFallback(handler: async (IUrlMappingRepository _repository, IHttpContextAccessor contextAccessor) =>
{
    var path = contextAccessor.HttpContext!.Request.Path.ToUriComponent().Trim('/').ToLower();
    var urlMatch = await _repository.FindByShortUrlAsync(path);
    return Results.Redirect(urlMatch.Url);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
