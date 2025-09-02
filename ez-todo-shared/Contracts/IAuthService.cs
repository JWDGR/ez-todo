using EzTodo.Shared.Models;

namespace EzTodo.Shared.Contracts;

public interface IAuthService
{
    bool CreateUser(User user, string password);
    User? ValidateUser(string email, string password);
    User? GetUserByEmail(string email);
    string GenerateJwtToken(User user);
}