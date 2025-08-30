import { Todo, TodoCreate, TodoUpdate } from "@/models/Todo";

const EZ_TODO_API_URL = process.env.NEXT_PUBLIC_EZ_TODO_API_URL;

export async function getTodos(): Promise<Todo[]> {
  try {
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo`);

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
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
    const response = await fetch(`${EZ_TODO_API_URL}/api/todo/${id}`);

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
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
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(todoCreateParams),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
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
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(todoUpdateParams),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
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
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
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
    });

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const toggledTodo = await response.json();
    return toggledTodo;
  } catch (error) {
    console.error("Error toggling todo:", error);
    throw error;
  }
}
