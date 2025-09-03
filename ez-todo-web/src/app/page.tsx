"use client";

import Link from "next/link";
import { useAuth } from "@/contexts/AuthContext";
import { useQuery } from "@tanstack/react-query";
import { getTodos } from "@/lib/api";

export default function Home() {
  const { isAuthenticated, user, logout } = useAuth();

  const {
    data: todos = [],
    isLoading,
    error,
  } = useQuery({
    queryKey: ["todos"],
    queryFn: getTodos,
    enabled: isAuthenticated, // Only fetch when authenticated
  });

  if (!isAuthenticated) {
    return (
      <div className="p-8 items-center flex flex-col">
        <h2 className="text-2xl font-bold mb-6">Welcome to EzTodo</h2>
        <p className="text-sm text-center p-10 items-center">
          Please login or sign up to manage your todos.
        </p>
        <div className="flex gap-4">
          <Link href="/login">Login</Link>
          <Link href="/signup">Sign Up</Link>
        </div>
      </div>
    );
  }

  return (
    <div className="p-8 items-center flex flex-col">
      <div className="flex justify-between items-center w-full max-w-4xl mb-6">
        <h1 className="text-2xl font-bold">Welcome, {user?.email}!</h1>
        <button onClick={logout} className="text-red-500">
          Logout
        </button>
      </div>

      {isLoading && <p>Loading todos...</p>}
      {error && <p>Error loading todos: {error.message}</p>}

      <div className="w-full max-w-4xl">
        <p className="text-sm text-center p-4">{todos.length} todos found.</p>
        {todos.length > 0 ? (
          <div className="flex gap-4 justify-center">
            <Link href="/todos">View All Todos</Link>
            <Link href="/todos/new">Create New Todo</Link>
          </div>
        ) : (
          <div className="flex justify-center">
            <Link href="/todos/new">Create Your First Todo</Link>
          </div>
        )}
      </div>
    </div>
  );
}
