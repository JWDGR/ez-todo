using System.ComponentModel.DataAnnotations;
using EzTodo.Auth;
using EzTodo.Shared.DTOs;
using EzTodo.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register AuthService with connection string from configuration
builder.Services.AddSingleton<AuthService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    return new AuthService(connectionString);
});

// Add CORS for cross-origin requests from Todo service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfigured", policy =>
    {
        var corsOrigins = builder.Configuration.GetSection("ApiSettings:CorsOrigins").Get<string[]>() ?? [];
        
        if (corsOrigins.Length > 0)
        {
            policy.WithOrigins(corsOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition", "Content-Length", "Content-Type");
        }
        else
        {
            // Fallback to allow all if no origins configured
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition", "Content-Length", "Content-Type");
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowConfigured");
app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

app.MapPost("/auth/signup", (AuthService authService, SignupRequest request) =>
{
    try
    {
        // Validate request
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
        {
            return Results.BadRequest(new { error = "Valid email address is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            return Results.BadRequest(new { error = "Password must be at least 6 characters long" });
        }
        
        var user = new User
        {
            Email = request.Email.Trim().ToLower(),
            Role = Role.User
        };

        var success = authService.CreateUser(user, request.Password);

        if (!success) return Results.BadRequest(new { error = "Failed to create user" });
        
        // Get the created user
        var createdUser = authService.GetUserByEmail(user.Email);
        if (createdUser == null) return Results.BadRequest(new { error = "Failed to create user" });
        
        var token = authService.GenerateJwtToken(createdUser);
        return Results.Ok(new
        {
            message = "User registered successfully",
            user = new
            {
                id = createdUser.Id,
                email = createdUser.Email,
                role = createdUser.Role,
                createdAt = createdUser.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            },
            token = token
        });

    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = $"Registration failed: {ex.Message}" });
    }
})
.WithName("Signup")
.WithOpenApi()
.WithSummary("Register a new user")
.WithDescription("Creates a new user account and returns an authentication token");

app.Run();

static bool IsValidEmail(string email)
{
    try
    {
        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }
    catch
    {
        return false;
    }
}
