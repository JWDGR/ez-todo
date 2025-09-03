# EZ Todo API

The EZ TODO application API built with ASP.NET Core 9.


## API Endpoints

# Auth apis

| Method | Endpoint                | Description               |
|--------|-------------------------|---------------------------|
| GET    | `/api/auth/login`       | Login user                |
| GET    | `/api/auth/signup`      | Signup and login          |
| POST   | `/api/auth/validate`    | Validate the token        |

# Todo apis

| Method | Endpoint                | Description               |
|--------|-------------------------|---------------------------|
| GET    | `/api/todo`             | Get all todos             |
| GET    | `/api/todo/{id}`        | Get a specific todo by ID |
| POST   | `/api/todo`             | Create a new todo         |
| PUT    | `/api/todo/{id}`        | Update an existing todo   |
| DELETE | `/api/todo/{id}`        | Delete a todo             |
| PATCH  | `/api/todo/{id}/toggle` | Toggle completion status  |

## The TODO Data Model
- `Id` (int): Unique identifier
- `Title` (string): Todo title (required)
- `Description` (string?): Optional description
- `IsCompleted` (bool): Completion status
- `CreatedAt` (DateTime): Creation timestamp

## Running the API

```bash
cd ez-todo-api
dotnet run
```

The API will be available at:
- **HTTP**: http://localhost:4000
- **HTTP**: https://localhost:7000
- **Swagger UI**: https://localhost:4000/swagger or https://localhost:7000/swagger

## Testing

Use the `todo-api.http` file in your IDE. I use Rider to test all endpoints.
