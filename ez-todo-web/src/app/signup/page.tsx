"use client";

import { useRouter } from "next/navigation";
import { useMutation } from "@tanstack/react-query";
import { SignupRequest } from "@/models/Auth";
import { signup } from "@/lib/api";
import { useAuth } from "@/contexts/AuthContext";
import Link from "next/link";

export default function SignupPage() {
  const router = useRouter();
  const { login: authLogin, isAuthenticated } = useAuth();

  const { mutate, isPending, isError, error } = useMutation({
    mutationFn: (signupRequest: SignupRequest) => signup(signupRequest),
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
    const confirmPassword = formData.get("confirmPassword") as string;

    if (password !== confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    if (password.length < 6) {
      alert("Password must be at least 6 characters long");
      return;
    }

    mutate({
      email,
      password,
    });
  };

  return (
    <div className="p-8 items-center flex flex-col">
      <h1 className="text-2xl font-bold mb-6">Create an account</h1>
      <form
        className="items-center flex flex-col space-y-4"
        onSubmit={handleSubmit}
      >
        {isPending && <p>Creating account...</p>}
        {isError && <p>Error: {error.message}</p>}
        <input type="email" name="email" placeholder="Email" required />
        <input
          type="password"
          name="password"
          placeholder="Password"
          required
        />
        <input
          type="password"
          name="confirmPassword"
          placeholder="Confirm Password"
          required
        />
        <button type="submit">Sign Up</button>
      </form>
      <p className="mt-4 text-sm">
        Already have an account?{" "}
        <Link href="/login" className="text-blue-500 hover:underline">
          Login
        </Link>
      </p>
    </div>
  );
}
