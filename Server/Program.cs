using Swashbuckle.AspNetCore;
using ExpenseTrackerNew.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS for Vite frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("https://localhost:53876")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


var app = builder.Build();

// Use Swagger (only in development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS redirection
app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowFrontend");

// Use Authorization
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();
