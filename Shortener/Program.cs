using MongoDB.Driver;
using dotenv.net;
using Shortener.Models;
using Shortener.Data;
using Shortener.Service;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
var envKeys = DotEnv.Read();

var mongoConnectionString = envKeys["ConnectionString"];
var mongoDatabaseName = envKeys["DatabaseName"];
var mongoCollectionName = envKeys["CollectionName"];

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