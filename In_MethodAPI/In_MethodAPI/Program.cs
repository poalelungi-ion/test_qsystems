using In_MethodAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();  // Adds MVC controllers to the DI container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("RequestLogsDb"));  // Add in-memory database
builder.Services.AddHttpClient();  // Add IHttpClientFactory for making external HTTP requests

// Add Swagger/OpenAPI services for API documentation
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
app.UseAuthorization();

// Map the endpoints using controller-based routes
app.MapControllers();

// Endpoint to get all request logs using minimal API
app.MapGet("/api/requests", async (ApplicationDbContext context) =>
{
    var logs = await context.RequestLogs.ToListAsync();
    return Results.Ok(logs);
});

app.Run();
