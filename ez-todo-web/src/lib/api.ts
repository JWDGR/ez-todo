import { Todo } from '@/models/Todo';

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
    console.error('Error fetching todos:', error);
    throw error;
  }
}
