import Image from "next/image";

export default function Home() {
  return (
    <div className="font-sans grid grid-rows-[20px_1fr_20px] items-center justify-items-center min-h-screen p-8 pb-20 gap-16 sm:p-20">
  <main className="flex flex-col gap-[32px] row-start-2 items-center">
        <Image
          className="dark:invert"
          src="/EzLogo.png"
          alt="EzTodo logo"
          width={180}
          height={38}
          priority
        />
        <p className="text-sm text-center sm:text-left">
          This is a simple todo app built with Next.js and Tailwind CSS.
        </p>
      </main>
      <footer className="row-start-3 flex gap-[24px] flex-wrap items-center justify-center">
       <p className="text-sm text-center sm:text-left">
         &copy; {new Date().getFullYear()} EzTodo. All rights reserved.
       </p>
      </footer>
    </div>
  );
}
