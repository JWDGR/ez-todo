import Link from "next/link";
import { getTodos } from "@/lib/api";
import { Todo } from "@/models/Todo";

export default async function Home() {
  const todos: Todo[] = await getTodos();

  return (
    <div className="p-8 items-center flex flex-col">
      <p className="text-sm text-center sm:text-left p-10 items-center">
        {todos.length} todos found.
      </p>
      {todos.length > 0 ? (
        <Link href="/todos">Go to details</Link>
      ) : (
        <Link href="/todos/new">Create a new todo</Link>
      )}
    </div>
  );
}
