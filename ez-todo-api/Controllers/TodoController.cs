using Microsoft.AspNetCore.Mvc;
using EzTodo.Shared.Models;
using EzTodo.Shared.DTOs;
using EzTodo.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace EzTodo.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController(ITodoServiceClient todoServiceClient) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        try
        {
            var todos = await todoServiceClient.GetAllTodosAsync();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving todos: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        try
        {
            var todo = await todoServiceClient.GetTodoByIdAsync(id);
            if (todo == null)
                return NotFound($"Todo with ID {id} not found");

            return Ok(todo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving todo: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create(CreateTodoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                return BadRequest("Title is required");

            var newTodo = await todoServiceClient.CreateTodoAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating todo: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItem>> Update(int id, UpdateTodoRequest request)
    {
        try
        {
            var todo = await todoServiceClient.UpdateTodoAsync(id, request);
            if (todo == null)
                return NotFound($"Todo with ID {id} not found");

            return Ok(todo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating todo: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var success = await todoServiceClient.DeleteTodoAsync(id);
            if (!success)
                return NotFound($"Todo with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting todo: {ex.Message}");
        }
    }

    [HttpPatch("{id}/toggle")]
    public async Task<ActionResult<TodoItem>> Toggle(int id)
    {
        try
        {
            var todo = await todoServiceClient.ToggleTodoAsync(id);
            if (todo == null)
                return NotFound($"Todo with ID {id} not found");

            return Ok(todo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error toggling todo: {ex.Message}");
        }
    }
}
