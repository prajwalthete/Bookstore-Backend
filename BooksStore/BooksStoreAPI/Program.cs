using BooksStoreAPI.Middleware;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<BookStoreContext>();
builder.Services.AddScoped<ICustomerBL, CustomerBL>();
builder.Services.AddScoped<ICustomerRL, CustomerRL>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
