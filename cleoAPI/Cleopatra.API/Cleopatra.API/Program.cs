using Microsoft.EntityFrameworkCore;
using Cleopatra.Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Cleopatra.API.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context configuration
builder.Services.AddDbContext<SalonContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5174") // React App URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Allow cookies
    });
});

// Authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.Cookie.Name = "CleopatraAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Requires HTTPS
        options.Cookie.SameSite = SameSiteMode.None; // Allow cross-origin cookies
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("manager"));
});

// Register services
builder.Services.AddScoped<NotificationService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Global exception handler
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An unhandled exception occurred.");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        await context.Response.WriteAsync(new
        {
            error = "An unhandled exception occurred.",
            details = ex.Message
        }.ToString());
    }
});

app.MapControllers();

app.Run();
