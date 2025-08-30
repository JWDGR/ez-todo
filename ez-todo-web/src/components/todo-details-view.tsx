"use client";

import { useQuery } from "@tanstack/react-query";
import { getTodo } from "@/lib/api";

interface TodoDetailsViewProps {
  id: string;
}

export default function TodoDetailsView({ id }: TodoDetailsViewProps) {
  const { data, isLoading, error } = useQuery({
    queryKey: ["todos", id],
    queryFn: () => getTodo(id),
  });

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;
  if (!data) return <div>Todo not found</div>;

  return (
    <div>
      <p>Title: {data.title}</p>
      <p>Description: {data.description}</p>
      <p>Is Completed: {data.isCompleted ? "Yes" : "No"}</p>
      <p>Created At: {data.createdAt}</p>
    </div>
  );
}
