"use client";

import { useRouter } from "next/navigation";
import { Todo, TodoUpdate } from "@/models/Todo";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { deleteTodo, getTodo, updateTodo } from "@/lib/api";

interface TodoFormEditProps {
  id: string;
}

export default function TodoFormEdit({ id }: TodoFormEditProps) {
  const router = useRouter();
  const queryClient = useQueryClient();

  const {
    data: todo,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["todos", id],
    queryFn: () => getTodo(id),
  });

  const {
    mutate: updateTodoMutation,
    isPending: isUpdatePending,
    isError: isUpdateError,
    error: updateError,
  } = useMutation({
    mutationFn: (params: TodoUpdate) => updateTodo(todo!.id, params),
    onSuccess: async () => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ["todos", todo!.id] }),
        queryClient.invalidateQueries({ queryKey: ["todos"] })
      ]);
      router.push(`/todos/${id}`);
    },
  });

  const {
    mutate: deleteTodoMutation,
    isPending: isDeletePending,
    isError: isDeleteError,
    error: deleteError,
  } = useMutation({
    mutationFn: () => deleteTodo(todo!.id),
    onSuccess: async () => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: ["todos", todo!.id] }),
        queryClient.invalidateQueries({ queryKey: ["todos"] })
      ]);
      router.push("/todos");
    },
  });

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;
  if (!todo) return <div>Todo not found</div>;

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const title = formData.get("title") as string;
    const description = formData.get("description") as string;
    const isCompleted = Boolean(formData.get("isCompleted")) as boolean;

    updateTodoMutation({
      title,
      description,
      isCompleted,
    });
  };

  const handleDelete = () => {
    deleteTodoMutation();
  };

  return (
    <form className="items-center flex flex-col" onSubmit={handleSubmit}>
      {isUpdatePending && <div>Updating...</div>}
      {isDeletePending && <div>Deleting...</div>}
      {isUpdateError && <div>Error: {updateError.message}</div>}
      {isDeleteError && <div>Error: {deleteError.message}</div>}
      <span>{todo.id}</span>
      <span>{todo.createdAt}</span>
      <label htmlFor="title">Title</label>
      <input
        type="text"
        name="title"
        placeholder="Title"
        required
        defaultValue={todo.title}
      />
      <label htmlFor="description">{todo.description}</label>
      <input
        type="text"
        name="description"
        placeholder="Description"
        defaultValue={todo.description}
      />
      <label htmlFor="isCompleted">{todo.isCompleted}</label>
      <input
        type="checkbox"
        name="isCompleted"
        defaultChecked={todo.isCompleted}
      />
      <button type="submit">Save</button>
      <button type="button" onClick={handleDelete}>
        Delete
      </button>
    </form>
  );
}
