using BooksStoreAPI.Middleware;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;

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

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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