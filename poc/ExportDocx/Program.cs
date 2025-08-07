using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FridgeMate.ExportDocx.Models;
using FridgeMate.ExportDocx.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Register app services
// If WordTemplateService has dependencies, change lifetime accordingly.
builder.Services.AddScoped<WordTemplateService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello, world!");

// Map controllers instead of inline minimal API endpoints
app.MapControllers();

app.Run();