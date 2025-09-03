using EzTodo.Shared.DTOs;
using EzTodo.Shared.Models;
using System.Text;
using System.Text.Json;

namespace EzTodo.Api.Services;

public interface IUserServiceClient
{
    Task<AuthResponse> SignupAsync(SignupRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}

public class UserServiceClient(IHttpClientFactory httpClientFactory) : IUserServiceClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("UserService");
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<AuthResponse> SignupAsync(SignupRequest request)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/auth/signup", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(responseContent, _jsonOptions) 
            ?? throw new InvalidOperationException("Failed to deserialize auth response");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AuthResponse>(responseContent, _jsonOptions) 
            ?? throw new InvalidOperationException("Failed to deserialize auth response");
    }
}

public class AuthResponse
{
    public string Message { get; set; } = string.Empty;
    public UserInfo User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
}

public class UserInfo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.User;
    public string CreatedAt { get; set; } = string.Empty;
}

