using shortfy_api.src.Application.Configuration;
using shortfy_api.src.Authentication.Endpoints;
using shortfy_api.User.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.RegisterServices();
builder.InjectDepencies();
builder.ConnectToDb();
builder.JwtAuthConfig();
builder.RegisterSections();


var app = builder.Build();

// Do Database Migration
app.DoMigration();

// Configure the HTTP request pipeline.
app.RegisterMiddlewares();
app.RegisterUserEndpoints();
app.RegisterAuthenticationEndpoints();
app.DisableCors();


app.Run();
