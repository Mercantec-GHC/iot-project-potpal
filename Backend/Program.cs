using Database;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


builder.Services.AddDbContext<PotPalDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<PotPalDbContext>(options => options.UseNpgsql(""));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PlantRepo>();
builder.Services.AddScoped<PlantService>();

builder.Services.AddControllers();

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

app.MapControllers();

app.Run();


