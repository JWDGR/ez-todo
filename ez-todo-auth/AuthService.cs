using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EzTodo.Shared.Models;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using EzTodo.Shared.Contracts;
using Microsoft.Extensions.Configuration;

namespace EzTodo.Auth;

public class AuthService : IAuthService
{
    private readonly string _connectionString;
    private readonly string _jwtSecret;
    private readonly int _jwtExpiryMinutes;
    private readonly string _jwtIssuer;
    
    public AuthService(string connectionString, IConfiguration configuration)
    {
        _connectionString = connectionString;
        
        // Read JWT settings from configuration with environment variable fallback
        _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") 
            ?? configuration["JwtSettings:Secret"] 
            ?? "super_secret_key_that_is_long_enough_for_jwt_hmac_sha256_algorithm";
        
        _jwtExpiryMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES") 
            ?? configuration["JwtSettings:ExpiryMinutes"] 
            ?? "60");
        
        _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
            ?? configuration["JwtSettings:Issuer"] 
            ?? "ez-todo-auth";
        
        InitializeDatabase();
    }
    
    public bool CreateUser(User user, string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long");
        }
        
        // TODO: validate if user already exists
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Name, Email, PasswordHash, Role, CreatedAt) VALUES (@Name, @Email, @PasswordHash, @Role, @CreatedAt)";
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
        command.Parameters.AddWithValue("@Role", user.Role);
        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
        
        var rowsAffected = command.ExecuteNonQuery();
        
        return rowsAffected > 0;
    }

    public User? ValidateUser(string email, string password)
    {
        var user = GetUserByEmail(email);
        
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
        {
            return null;
        }

        return !VerifyPassword(password, user.PasswordHash) ? null : user;
    }
    
    public User? GetUserByEmail(string email)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Users WHERE Email = @email";
        selectCmd.Parameters.AddWithValue("@email", email.ToLower());

        using var reader = selectCmd.ExecuteReader();
        return reader.Read() ? MapReaderToUser(reader) : null;
    }
    
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSecret);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes),
            Issuer = _jwtIssuer,
            Audience = _jwtIssuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    private void InitializeDatabase()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Email TEXT, PasswordHash TEXT, Role TEXT, CreatedAt TEXT)";
        command.ExecuteNonQuery();
        CreateDefaultAdminUser();
    }

    private void CreateDefaultAdminUser()
    {
        try
        {
            var adminUser = new User
            {
                Email = "admin@eztodo.com",
                Role = Role.Admin,
                CreatedAt = DateTime.UtcNow
            };
            
            var access = CreateUser(adminUser, "userAdminPassword");

            Console.WriteLine(access ? "Admin user created successfully" : "Admin user already exists");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt_key_for_security"));
        return Convert.ToBase64String(hashedBytes);
    }
    
    private static bool VerifyPassword(string password, string hash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == hash;
    }

    private User MapReaderToUser(SqliteDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            Role = (Role)Enum.Parse(typeof(Role), reader.GetString(4)),
            CreatedAt = DateTime.Parse(reader.GetString(5))
        };
    }
}