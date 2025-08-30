export interface Todo {
  id: string;
  title: string;
  description?: string;
  isCompleted: boolean;
  createdAt: string;
}

export interface TodoCreate {
  title: string;
  description?: string;
}

export interface TodoUpdate {
  title?: string;
  description?: string;
  isCompleted?: boolean;
}
