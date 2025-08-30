using Serilog;
using StampedeProtection.Api.Features;
using StampedeProtection.Api.Shared.Application.Repositories;
using StampedeProtection.Api.Shared.InfraStructure;
using StampedeProtection.Api.Shared.InfraStructure.Repos;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Minute)
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHybridCache();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<GetProductRequestHandler>();
builder.Services.AddScoped<GetProductWithMemoryRequestHandler>();
builder.Services.AddScoped<GetProductWithHybridRequestHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.AddGetProductEndPoint();
app.AddGetProductWithMemoryEndPoint();
app.AddGetProductWithHybridEndPoint();


app.Run();

public partial class Program { }