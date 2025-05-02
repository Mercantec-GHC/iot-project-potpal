using Database;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add configuration settings
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Add DbContext and connection string
builder.Services.AddDbContext<PotPalDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PlantRepo>();
builder.Services.AddScoped<PlantService>();
builder.Services.AddScoped<MetricRepo>();
builder.Services.AddScoped<MetricService>();

// Add controllers and configure JSON serialization options
builder.Services.AddControllers(options =>
{
    // Optional: You can configure other options if needed
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
