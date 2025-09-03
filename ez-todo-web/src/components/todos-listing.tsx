"use client";

import { useRouter } from "next/navigation";
import { useQuery } from "@tanstack/react-query";
import { getTodos } from "@/lib/api";
import Link from "next/link";

export default function TodosListing() {
  const router = useRouter();
  const { data, isLoading, error } = useQuery({
    queryKey: ["todos"],
    queryFn: () => getTodos(),
  });

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;
  if (!data?.length) return <div>No todos found</div>;

  const handleNavigateToCreateTodo = () => {
    router.push("/todos/new");
  };

  return (
    <div>
      <ol className="list-none m-8">
        {data.map((todo) => (
          <li key={todo.id}>
            <Link href={`/todos/${todo.id}`}>{todo.title}</Link>
          </li>
        ))}
      </ol>
      <button onClick={handleNavigateToCreateTodo}>Create a new Todo</button>
    </div>
  );
}
