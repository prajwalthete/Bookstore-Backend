using BooksStoreAPI.Middleware;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System.Text;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{


    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    // Add services to the container.
    builder.Services.AddSingleton<BookStoreContext>();
    builder.Services.AddScoped<ICustomerBL, CustomerBL>();
    builder.Services.AddScoped<ICustomerRL, CustomerRL>();

    builder.Services.AddScoped<IAuthService, AuthService>();

    // Get the secret key from the configuration
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]);

    // Add authentication services with JWT Bearer token validation to the service collection
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)


        // Add JWT Bearer authentication options
        .AddJwtBearer(options =>
        {
            // Configure token validation parameters
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Specify whether the server should validate the signing key
                ValidateIssuerSigningKey = true,

                // Set the signing key to verify the JWT signature
                IssuerSigningKey = new SymmetricSecurityKey(key),

                // Specify whether to validate the issuer of the token (usually set to false for development)
                ValidateIssuer = false,

                // Specify whether to validate the audience of the token (usually set to false for development)
                ValidateAudience = false
            };
        });


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // Define Swagger document metadata (title and version)
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        // Configure JWT authentication for Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            // Describe how to pass the token
            Description = "JWT Authorization header using the Bearer scheme",
            Name = "Authorization", // The name of the header containing the JWT token
            In = ParameterLocation.Header, // Location of the JWT token in the request headers
            Type = SecuritySchemeType.Http, // Specifies the type of security scheme (HTTP in this case)
            Scheme = "bearer", // The authentication scheme to be used (in this case, "bearer")
            BearerFormat = "JWT" // The format of the JWT token
        });

        // Specify security requirements for Swagger endpoints
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // Define a reference to the security scheme defined above
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // The ID of the security scheme (defined in AddSecurityDefinition)
                }
            },
            new string[] {} // Specify the required scopes (in this case, none)
        }
    });
    });


    // Configure NLog for logging
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Logging.AddNLog(new NLogProviderOptions
    {
        CaptureMessageTemplates = true,
        CaptureMessageProperties = true
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Add the global error handling middleware
    app.UseErrorHandlingMiddleware();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}