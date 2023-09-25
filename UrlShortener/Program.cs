using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using MongoDB.Driver;
using SharedModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var mongoClient = new MongoClient("mongodb://url-shortener-db:tB7kQlucl34G8prxrOMtgnDuxSjQTGsKAbsdY4ghiwgBKzCf2BSj9t7SAQZ3jnGtJzZVvoUYI2SsACDbwnEeYg%3D%3D@url-shortener-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@url-shortener-db@");
var mongoDatabase = mongoClient.GetDatabase("urlShortener");
builder.Services.AddSingleton<IMongoCollection<UrlMapping>>(ProviderAliasAttribute => mongoDatabase.GetCollection<UrlMapping>("shorteners"));

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
