using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using suavesabor_api.Application.Data;
using suavesabor_api.Application.Repository.Generic;
using suavesabor_api.Application.Repository.Generic.Impl;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;
using suavesabor_api.User.Repository.Impl;
using suavesabor_api.User.UseCase;
using suavesabor_api.User.UseCase.Impl;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace suavesabor_api.Application.Configuration
{
    public static class Configuration
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services
                   .AddEndpointsApiExplorer()
                   .AddSwaggerGen()
                   .Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
                   {
                       options.SerializerOptions.PropertyNameCaseInsensitive = false;
                       options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                       options.SerializerOptions.WriteIndented = true;
                       options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                       options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                   }); ;
        }

        public static void InjectDepencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
            builder.Services.AddScoped(typeof(IGenericRepository<UserDomain, ObjectId>), typeof(GenericRepositoryImpl<UserDomain, ObjectId>));
            builder.Services.AddScoped<ISearchUserUseCase, SearchUserUseCaseImpl>();
            builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCaseImpl>();
        }

        public static void ConnectToMongoDb(this WebApplicationBuilder builder)
        {
            var environmentVariableName = "MONGODB_URI";
            var connectionString = Environment.GetEnvironmentVariable(environmentVariableName);
            if (connectionString == null)
            {
                Console.WriteLine($"You must set your ${environmentVariableName} environment variable.");
                Environment.Exit(0);
            }

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseMongoDB(connectionString, "suave_sabor");
            });

        }

        public static void RegisterMiddlewares(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger()
                   .UseSwaggerUI();
            }
            //app.UseHttpsRedirection();
        }
    }
}
