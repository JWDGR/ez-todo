using Microsoft.AspNetCore.Mvc;
using EzTodo.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace EzTodo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    // Start with in-memory storage for tests
    private static readonly List<Todo> _todos = new();
    private static int _nextId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Todo>> GetAll()
    {
        return Ok(_todos);
    }

    [HttpGet("{id}")]
    public ActionResult<Todo> GetById(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound($"Todo with ID {id} not found");

        return Ok(todo);
    }

    [HttpPost]
    public ActionResult<Todo> Create(CreateTodoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest("Title is required");

        var newTodo = new Todo
        {
            Id = _nextId++,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _todos.Add(newTodo);
        return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
    }

    [HttpPut("{id}")]
    public ActionResult<Todo> Update(int id, UpdateTodoRequest request)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound($"Todo with ID {id} not found");

        if (!string.IsNullOrWhiteSpace(request.Title))
            todo.Title = request.Title;

        if (request.Description != null)
            todo.Description = request.Description;

        if (request.IsCompleted.HasValue)
            todo.IsCompleted = request.IsCompleted.Value;

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound($"Todo with ID {id} not found");

        _todos.Remove(todo);
        return NoContent();
    }

    [HttpPatch("{id}/toggle")]
    public ActionResult<Todo> Toggle(int id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
            return NotFound($"Todo with ID {id} not found");

        todo.IsCompleted = !todo.IsCompleted;
        return Ok(todo);
    }
}
