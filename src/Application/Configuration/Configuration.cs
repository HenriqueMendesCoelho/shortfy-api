using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using shortfy_api.src.Application.Data;
using shortfy_api.src.Application.Repository.Generic;
using shortfy_api.src.Application.Repository.Generic.Impl;
using shortfy_api.src.Authentication.Endpoints.Dto;
using shortfy_api.src.Authentication.Endpoints.Dto.Validators;
using shortfy_api.src.Authentication.UseCase;
using shortfy_api.src.Authentication.UseCase.Impl;
using shortfy_api.src.User.Endpoints.Dto;
using shortfy_api.src.User.Endpoints.Dto.Validators;
using shortfy_api.src.User.UseCase;
using shortfy_api.src.User.UseCase.Impl;
using shortfy_api.User.Domain;
using shortfy_api.User.Endpoints.Dto;
using shortfy_api.User.Endpoints.Dto.Validators;
using shortfy_api.User.Repository;
using shortfy_api.User.Repository.Impl;
using shortfy_api.User.UseCase;
using shortfy_api.User.UseCase.Impl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace shortfy_api.src.Application.Configuration
{
    public static class Configuration
    {
        private static ILogger logger = LoggerFactory.Create(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Trace)
        .AddConsole())
            .CreateLogger<Program>();

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
                   });
            builder.Services.AddCors();
            builder.Services.AddTransient<ILogger>(p =>
            {
                var loggerFactory = p.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger("logger");
            });
        }

        public static void InjectDepencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IGenericRepository<UserDomain, Guid>), typeof(GenericRepositoryImpl<UserDomain, Guid>));
            builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();

            builder.Services.AddScoped<ISearchUserUseCase, SearchUserUseCaseImpl>();
            builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCaseImpl>();
            builder.Services.AddScoped<ICreateTokenUseCase, CreateTokenUseCaseImpl>();
            builder.Services.AddScoped<ILoginUseCase, LoginUseCaseImpl>();
            builder.Services.AddScoped<ICreateUserLoginUseCase, CreateUserLoginUseCaseImpl>();
            builder.Services.AddScoped<ILoginGoogleUseCase, LoginGoogleUseCaseImpl>();
            builder.Services.AddScoped<ILoginMicrosoftUseCase, LoginMicrosoftUseCaseImpl>();
            builder.Services.AddScoped<IGetPrincipalTokenUseCase, GetPrincipalTokenUseCaseImpl>();
            builder.Services.AddScoped<ICreateRefreshTokenUseCase, CreateRefreshTokenUseCaseImpl>();
            builder.Services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCaseImpl>();
            builder.Services.AddScoped<IUpdateUserUseCase, UpdateUserUseCaseImpl>();
            builder.Services.AddScoped<IDeleteUserUseCase, DeleteUserUseCaseImpl>();
            builder.Services.AddScoped<IPromoteUserUseCase, PromoteUserUseCaseImpl>();
            builder.Services.AddScoped<IDemoteUserUseCase, DemoteUserUseCaseImpl>();

            builder.Services.AddTransient<IValidator<UserRequestDto>, UserRequestDtoValidator>();
            builder.Services.AddTransient<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
            builder.Services.AddTransient<IValidator<LoginOAuthRequestDto>, LoginOAuthRequestDtoValidator>();
            builder.Services.AddTransient<IValidator<RefreshRequestDto>, RefreshTokenRequestDtoValidator>();
            builder.Services.AddTransient<IValidator<UserUpdateRequestDto>, UserUpdateRequestDtoValidator>();

            builder.Services.AddFluentValidationRulesToSwagger();
        }

        public static void ConnectToDb(this WebApplicationBuilder builder)
        {
            var environmentVariableName = "DB-URL";
            var connectionString = Environment.GetEnvironmentVariable(environmentVariableName);
            if (connectionString == null)
            {
                Console.WriteLine($"You must set your '${environmentVariableName}' environment variable. \n");
                Console.WriteLine("Example: ");
                Console.WriteLine("Host=<host>:<port>;Database=<database>;Username=<databaseUsername>;Password=<databasePassword>");
                Environment.Exit(0);
            }

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

        }

        public static void RegisterSections(this WebApplicationBuilder builder)
        {
            var googleConfiguration = builder.Configuration.GetSection("GoogleConfiguration").Get<GoogleConfiguration>();
            var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");

            if (string.IsNullOrEmpty(googleClientId))
            {
                logger.LogError("You must set your 'GOOGLE_CLIENT_ID' environment variable. \n");
                Environment.Exit(10);
            }
            if (googleConfiguration is null)
            {
                logger.LogError("GoogleConfiguration is null.");
                Environment.Exit(10);
            }

            googleConfiguration.ClientId = googleClientId;
            builder.Services.AddSingleton(googleConfiguration);

            var microsoftEntraIdConfiguration = builder.Configuration.GetSection("MicrosoftEntraIdConfiguration").Get<MicrosoftEntraIdConfiguration>();
            var microsoftClientId = Environment.GetEnvironmentVariable("MICROSOFT_CLIENT_ID");
            if (string.IsNullOrEmpty(microsoftClientId))
            {
                logger.LogError("You must set your 'MICROSOFT_CLIENT_ID' environment variable. \n");
                Environment.Exit(10);
            }
            if (microsoftEntraIdConfiguration is null)
            {
                logger.LogError("MicrosoftEntraIdConfiguration is null.");
                Environment.Exit(10);
            }

            microsoftEntraIdConfiguration.ClientId = microsoftClientId;
            builder.Services.AddSingleton(microsoftEntraIdConfiguration);
        }

        public static void JwtAuthConfig(this WebApplicationBuilder builder)
        {
            var token = builder.Configuration.GetSection("TokenConfiguration").Get<TokenConfiguration>();
            var tokenEnvSecret = Environment.GetEnvironmentVariable("JWT_SECRET");

            if (tokenEnvSecret is null)
            {
                logger.LogError("You must set your 'JWT_SECRET' environment variable. \n");
                Environment.Exit(10);
            }
            if (token is not null)
            {
                token.Secret = tokenEnvSecret;
                builder.Services.AddSingleton(token);
            }
            else
            {
                logger.LogError("TokenConfiguration is null.");
                Environment.Exit(10);
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Secret))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN",
                authBuilder =>
                {
                    authBuilder.RequireRole("ADMIN");
                });
                options.AddPolicy("USER",
                authBuilder =>
                {
                    authBuilder.RequireRole("USER");
                });
            });
        }

        public static void RegisterMiddlewares(this WebApplication app)
        {

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger()
                   .UseSwaggerUI((settings) =>
                   {
                       settings.SwaggerEndpoint("/swagger/v1/swagger.yaml", "v1-yaml");
                       settings.SwaggerEndpoint("/swagger/v1/swagger.json", "v1-json");
                   });
            }
            //app.UseHttpsRedirection();
        }

        public static void DisableCors(this WebApplication app)
        {
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        }

        public static void DoMigration(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            dataContext.Database.Migrate();
        }
    }
}
