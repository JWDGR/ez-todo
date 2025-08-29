# EZ Todo API

The EZ TODO application API built with ASP.NET Core 9.


## API Endpoints

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
- **HTTP**: http://localhost:5229
- **HTTP**: https://localhost:7225
- **Swagger UI**: https://localhost:5229/swagger or https://localhost:7225/swagger

## Testing

Use the `todo-api.http` file in your IDE. I use Rider to test all endpoints.
