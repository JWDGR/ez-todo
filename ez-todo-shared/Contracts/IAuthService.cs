using EzTodo.Shared.Models;

namespace EzTodo.Shared.Contracts;

public interface IAuthService
{
    Task<User> AuthenticateAsync(string email, string password);
    Task<User> RegisterAsync(User user);
}