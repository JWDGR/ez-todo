using EzTodo.Shared.Models;
using EzTodo.Shared.DTOs;
using Microsoft.Data.Sqlite;

namespace EzTodo.Services;

public class TodoService
{
    private readonly string _connectionString;
    
    public TodoService(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }
    
    public IEnumerable<TodoItem> GetAllTodos()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Todos ORDER BY CreatedAt DESC";
        
        using var reader = command.ExecuteReader();
        var todos = new List<TodoItem>();
        
        while (reader.Read())
        {
            todos.Add(MapReaderToTodo(reader));
        }
        
        return todos;
    }
    
    public TodoItem? GetTodoById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Todos WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);
        
        using var reader = command.ExecuteReader();
        return reader.Read() ? MapReaderToTodo(reader) : null;
    }
    
    public TodoItem CreateTodo(CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required");
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt) VALUES (@Title, @Description, @IsCompleted, @CreatedAt); SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("@Title", request.Title);
        command.Parameters.AddWithValue("@Description", request.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@IsCompleted", false);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
        
        var id = Convert.ToInt32(command.ExecuteScalar());
        
        return new TodoItem
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public TodoItem? UpdateTodo(int id, UpdateTodoRequest request)
    {
        var existingTodo = GetTodoById(id);
        if (existingTodo == null)
            return null;
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Todos SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Title", !string.IsNullOrWhiteSpace(request.Title) ? request.Title : existingTodo.Title);
        command.Parameters.AddWithValue("@Description", request.Description ?? existingTodo.Description ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@IsCompleted", request.IsCompleted ?? existingTodo.IsCompleted);
        
        var rowsAffected = command.ExecuteNonQuery();
        
        if (rowsAffected > 0)
        {
            return GetTodoById(id);
        }
        
        return null;
    }
    
    public bool DeleteTodo(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Todos WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);
        
        var rowsAffected = command.ExecuteNonQuery();
        return rowsAffected > 0;
    }
    
    public TodoItem? ToggleTodo(int id)
    {
        var todo = GetTodoById(id);
        if (todo == null)
            return null;
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Todos SET IsCompleted = @IsCompleted WHERE Id = @Id";
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@IsCompleted", !todo.IsCompleted);
        
        var rowsAffected = command.ExecuteNonQuery();
        
        if (rowsAffected > 0)
        {
            return GetTodoById(id);
        }
        
        return null;
    }
    
    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Todos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                IsCompleted INTEGER NOT NULL DEFAULT 0,
                CreatedAt TEXT NOT NULL
            )";
        command.ExecuteNonQuery();
    }
    
    private TodoItem MapReaderToTodo(SqliteDataReader reader)
    {
        return new TodoItem
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
            IsCompleted = reader.GetInt32(3) == 1,
            CreatedAt = DateTime.Parse(reader.GetString(4))
        };
    }
}