"use client";

import { useQuery } from "@tanstack/react-query";
import TodoFormEdit from "./todo-form-edit";
import { getTodos } from "@/lib/api";

export default function TodosListing() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["todos"],
    queryFn: () => getTodos(),
  });

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;
  if (!data?.length) return <div>No todos found</div>;

  return (
    <div>
      <ol className="list-none m-8">
        {data.map((todo) => (
          <TodoFormEdit key={todo.id} todo={todo} />
        ))}
      </ol>
      <button>Add</button>
    </div>
  );
}
