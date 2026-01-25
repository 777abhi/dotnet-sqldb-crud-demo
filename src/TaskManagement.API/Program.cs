using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=tasks.db";

// DbContext
builder.Services.AddDbContext<TaskContext>(options =>
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        // Production Best Practice: Enable Connection Resilience
        // Note: SQLite does not support EnableRetryOnFailure() as it is a local file database.
        // For SQL Server, you would use: options.EnableRetryOnFailure();
    }));

// Register DataSeeder
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TaskContext>();
        // Attempt to seed if the database exists
        if (context.Database.CanConnect())
        {
             var seeder = services.GetRequiredService<DataSeeder>();
             seeder.Seed();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
