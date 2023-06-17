using MongoDB.Driver;
using dotenv.net;
using Shortener.Models;
using Shortener.Data;
using Shortener.Service;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
var envKeys = DotEnv.Read();

var mongoConnectionString = "mongodb://url-shortener-db:tB7kQlucl34G8prxrOMtgnDuxSjQTGsKAbsdY4ghiwgBKzCf2BSj9t7SAQZ3jnGtJzZVvoUYI2SsACDbwnEeYg%3D%3D@url-shortener-db.mongo.cosmos.azure.com:10255/?ssl=true&replicaSet=globaldb&retrywrites=false&maxIdleTimeMS=120000&appName=@url-shortener-db@";
var mongoDatabaseName = "urlShortener";
var mongoCollectionName = "shorteners";

var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
var mongoCollection = mongoDatabase.GetCollection<UrlMapping>(mongoCollectionName);
builder.Services.AddSingleton<IMongoCollection<UrlMapping>>(mongoCollection);
builder.Services.AddScoped<IShortenersRepository, ShortenersRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("default",
    policy =>
    {
        policy.WithOrigins("http://localhost:5200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("default");
app.MapControllers();

app.Run();