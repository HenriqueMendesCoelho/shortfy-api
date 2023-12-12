using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using suavesabor_api.Configuration;
using suavesabor_api.Data;
using suavesabor_api.Endpoints.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.RegisterServices();
builder.InjectDepencies();

var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
if (connectionString == null)
{
    Console.WriteLine("You must set your 'MONGODB_URI' environment variable.");
    Environment.Exit(0);
}

var client = new MongoClient("mongodb://mongodb:y7EJC38qNd6V@localhost:27017/?authMechanism=DEFAULT");
var db = client.GetDatabase("suave_sabor");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMongoDB("mongodb://mongodb:y7EJC38qNd6V@localhost:27017/", "suave_sabor");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.RegisterMiddlewares();
app.RegisterUserEndpoints();


app.Run();
