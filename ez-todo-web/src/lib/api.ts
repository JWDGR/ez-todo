import { Todo, TodoCreate, TodoUpdate } from "@/models/Todo";
import { LoginRequest, SignupRequest, AuthResponse } from "@/models/Auth";

const EZ_TODO_API_URL = process.env.NEXT_PUBLIC_EZ_TODO_API_URL;

// Helper function to get auth headers
function getAuthHeaders(): HeadersInit {
  const token = typeof window !== 'undefined' ? localStorage.getItem('authToken') : null;
  const headers: HeadersInit = {
    'Content-Type': 'application/json',
  };
  
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  
  return headers;
}

export async function getTodos(): Promise<Todo[]> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo`, {
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const todos = await response.json();
    return todos;
  } catch (error) {
    console.error("Error fetching todos:", error);
    throw error;
  }
}

export async function getTodo(id: string): Promise<Todo> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo/${id}`, {
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const todo = await response.json();
    return todo;
  } catch (error) {
    console.error("Error fetching todo:", error);
    throw error;
  }
}

export async function createTodo(todoCreateParams: TodoCreate): Promise<Todo> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(todoCreateParams),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const newTodo = await response.json();
    return newTodo;
  } catch (error) {
    console.error("Error creating todo:", error);
    throw error;
  }
}

export async function updateTodo(id: string, todoUpdateParams: TodoUpdate): Promise<Todo> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo/${id}`, {
      method: "PUT",
      headers: getAuthHeaders(),
      body: JSON.stringify(todoUpdateParams),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const updatedTodo = await response.json();
    return updatedTodo;
  } catch (error) {
    console.error("Error updating todo:", error);
    throw error;
  }
}

export async function deleteTodo(id: string): Promise<void> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo/${id}`, {
      method: "DELETE",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    return;
  } catch (error) {
    console.error("Error deleting todo:", error);
    throw error;
  }
}

export async function toggleTodo(id: string): Promise<Todo> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo/${id}/toggle`, {
      method: "POST",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const toggledTodo = await response.json();
    return toggledTodo;
  } catch (error) {
    console.error("Error toggling todo:", error);
    throw error;
  }
}

export async function login(loginRequest: LoginRequest): Promise<AuthResponse> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/user/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(loginRequest),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const authResponse = await response.json();
    return authResponse;
  } catch (error) {
    console.error("Error logging in:", error);
    throw error;
  }
}

export async function signup(signupRequest: SignupRequest): Promise<AuthResponse> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/user/signup`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(signupRequest),
    });

    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      const errorMessage = errorData.error || `HTTP error! status: ${response.status}`;
      throw new Error(errorMessage);
    }

    const authResponse = await response.json();
    return authResponse;
  } catch (error) {
    console.error("Error signing up:", error);
    throw error;
  }
}
