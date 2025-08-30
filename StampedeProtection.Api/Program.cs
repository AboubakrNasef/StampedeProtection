using StampedeProtection.Api.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GetProductRequestHandler>();
builder.Services.AddSingleton<GetProductWithMemoryRequestHandler>();
builder.Services.AddSingleton<GetProductWithHybridRequestHandler>();
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

