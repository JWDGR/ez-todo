"use client";

import { useRouter } from "next/navigation";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { TodoCreate } from "@/models/Todo";
import { createTodo } from "@/lib/api";

export default function TodoFormCreate() {
  const queryClient = useQueryClient();
  const router = useRouter();

  const { mutate, isPending, isError, error } = useMutation({
    mutationFn: (todo: TodoCreate) => createTodo(todo),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey: ["todos"] });
      router.push("/todos");
    },
  });

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const title = formData.get("title") as string;
    const description = formData.get("description") as string;

    mutate({
      title,
      description,
    });
  };

  return (
    <form
      className="items-center flex flex-col space-y-4"
      onSubmit={handleSubmit}
    >
      {isPending && <p>Creating todo...</p>}
      {isError && <p>Error: {error.message}</p>}
      <input type="text" name="title" placeholder="Title" required />
      <input type="text" name="description" placeholder="Description" />
      <button type="submit">Create</button>
    </form>
  );
}
