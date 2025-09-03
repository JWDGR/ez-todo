using EzTodo.Shared.Models;
using EzTodo.Shared.DTOs;
using System.Text;
using System.Text.Json;

namespace EzTodo.Api.Services;

public interface ITodoServiceClient
{
    Task<IEnumerable<TodoItem>> GetAllTodosAsync();
    Task<TodoItem?> GetTodoByIdAsync(int id);
    Task<TodoItem> CreateTodoAsync(CreateTodoRequest request);
    Task<TodoItem?> UpdateTodoAsync(int id, UpdateTodoRequest request);
    Task<bool> DeleteTodoAsync(int id);
    Task<TodoItem?> ToggleTodoAsync(int id);
}

public class TodoServiceClient(IHttpClientFactory httpClientFactory) : ITodoServiceClient
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("TodoService");
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<IEnumerable<TodoItem>> GetAllTodosAsync()
    {
        var response = await _httpClient.GetAsync("/todo");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<TodoItem>>(content, _jsonOptions) ?? new List<TodoItem>();
    }

    public async Task<TodoItem?> GetTodoByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/todo/{id}");
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
            
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TodoItem>(content, _jsonOptions);
    }

    public async Task<TodoItem> CreateTodoAsync(CreateTodoRequest request)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/todo", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TodoItem>(responseContent, _jsonOptions) 
            ?? throw new InvalidOperationException("Failed to deserialize created todo");
    }

    public async Task<TodoItem?> UpdateTodoAsync(int id, UpdateTodoRequest request)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync($"/todo/{id}", content);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
            
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TodoItem>(responseContent, _jsonOptions);
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/todo/{id}");
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return false;
            
        response.EnsureSuccessStatusCode();
        return true;
    }

    public async Task<TodoItem?> ToggleTodoAsync(int id)
    {
        var response = await _httpClient.PatchAsync($"/todo/{id}/toggle", null);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
            
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TodoItem>(content, _jsonOptions);
    }
}
