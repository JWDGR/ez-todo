using EzTodo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register TodoService with connection string from configuration
builder.Services.AddSingleton<TodoService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    return new TodoService(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithOpenApi();

// Todo endpoints
app.MapGet("/todo", (TodoService todoService) =>
{
    try
    {
        var todos = todoService.GetAllTodos();
        return Results.Ok(todos);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving todos: {ex.Message}");
    }
})
.WithName("GetAllTodos")
.WithOpenApi();

app.MapGet("/todo/{id:int}", (int id, TodoService todoService) =>
{
    try
    {
        var todo = todoService.GetTodoById(id);
        return todo == null ? Results.NotFound($"Todo with ID {id} not found") : Results.Ok(todo);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving todo: {ex.Message}");
    }
})
.WithName("GetTodoById")
.WithOpenApi();

app.MapPost("/todo", (EzTodo.Shared.DTOs.CreateTodoRequest request, TodoService todoService) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Results.BadRequest("Title is required");
        
        var todo = todoService.CreateTodo(request);
        return Results.Created($"/todo/{todo.Id}", todo);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error creating todo: {ex.Message}");
    }
})
.WithName("CreateTodo")
.WithOpenApi();

app.MapPut("/todo/{id:int}", (int id, EzTodo.Shared.DTOs.UpdateTodoRequest request, TodoService todoService) =>
{
    try
    {
        var todo = todoService.UpdateTodo(id, request);
        if (todo == null)
            return Results.NotFound($"Todo with ID {id} not found");
        
        return Results.Ok(todo);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error updating todo: {ex.Message}");
    }
})
.WithName("UpdateTodo")
.WithOpenApi();

app.MapDelete("/todo/{id:int}", (int id, TodoService todoService) =>
{
    try
    {
        var success = todoService.DeleteTodo(id);
        if (!success)
            return Results.NotFound($"Todo with ID {id} not found");
        
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error deleting todo: {ex.Message}");
    }
})
.WithName("DeleteTodo")
.WithOpenApi();

app.MapPatch("/todo/{id:int}/toggle", (int id, TodoService todoService) =>
{
    try
    {
        var todo = todoService.ToggleTodo(id);
        return todo == null ? Results.NotFound($"Todo with ID {id} not found") : Results.Ok(todo);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error toggling todo: {ex.Message}");
    }
})
.WithName("ToggleTodo")
.WithOpenApi();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5020";
app.Run($"http://0.0.0.0:{port}");