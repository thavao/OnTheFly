using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnTheFly.SalesAPI.Data;
using RabbitMQ.Client;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OnTheFlySalesAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnTheFlySalesAPIContext") ?? throw new InvalidOperationException("Connection string 'OnTheFlySalesAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();

// Register the ConnectionFactory as a singleton
builder.Services.AddSingleton<ConnectionFactory>();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
