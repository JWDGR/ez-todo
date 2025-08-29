import Link from "next/link";
export default function Home() {
  return (
    <p className="text-sm text-center sm:text-left p-10 items-center">
      See <Link href="/todos">todos</Link> page.
    </p>
  );
}
