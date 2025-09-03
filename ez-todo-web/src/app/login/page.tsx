"use client";

import { useRouter } from "next/navigation";
import { useMutation } from "@tanstack/react-query";
import { LoginRequest } from "@/models/Auth";
import { login } from "@/lib/api";
import { useAuth } from "@/contexts/AuthContext";
import Link from "next/link";

export default function LoginPage() {
  const router = useRouter();
  const { login: authLogin, isAuthenticated } = useAuth();

  const { mutate, isPending, isError, error } = useMutation({
    mutationFn: (loginRequest: LoginRequest) => login(loginRequest),
    onSuccess: (data) => {
      authLogin(data.user, data.token);
      router.push("/");
    },
  });

  if (isAuthenticated) {
    router.push("/");
    return null;
  }

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const email = formData.get("email") as string;
    const password = formData.get("password") as string;

    mutate({
      email,
      password,
    });
  };

  return (
    <div className="p-8 items-center flex flex-col">
      <h1 className="text-2xl font-bold mb-6">Welcome back</h1>
      <form
        className="items-center flex flex-col space-y-4"
        onSubmit={handleSubmit}
      >
        {isPending && <p>Logging in...</p>}
        {isError && <p>Error: {error.message}</p>}
        <input type="email" name="email" placeholder="Email" required />
        <input
          type="password"
          name="password"
          placeholder="Password"
          required
        />
        <button type="submit">Login</button>
      </form>
      <p className="mt-4 text-sm">
        Don&apos;t have an account?{" "}
        <Link href="/signup" className="text-blue-500 hover:underline">
          Sign up
        </Link>
      </p>
    </div>
  );
}
