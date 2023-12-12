using MongoDB.Bson;
using suavesabor_api.Domain.User;
using suavesabor_api.Repository;
using suavesabor_api.Repository.Generic;
using suavesabor_api.Repository.Generic.Impl;
using suavesabor_api.Repository.Impl;
using suavesabor_api.UseCase.User;
using suavesabor_api.UseCase.User.Impl;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace suavesabor_api.Configuration
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
