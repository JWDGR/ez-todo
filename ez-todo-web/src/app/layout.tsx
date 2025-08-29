import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import Link from "next/link";
import Image from "next/image";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "EzTodo",
  description:
    "Your elegant Todo App",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body
        className={`${geistSans.variable} ${geistMono.variable} antialiased min-h-screen flex flex-col`}
      >
        <header className="flex justify-center p-8">
          <Link href="/">
            <Image
              className="dark:invert"
              src="/EzLogo.png"
              alt="EzTodo logo"
              width={42}
              height={42}
              priority
            />
          </Link>
        </header>
        <main>{children}</main>
        <footer className="mt-auto py-6 flex gap-[24px] flex-wrap items-center justify-center">
          <p className="text-sm text-center">
            &copy; {new Date().getFullYear()} EzTodo. All rights reserved.
          </p>
        </footer>
      </body>
    </html>
  );
}
