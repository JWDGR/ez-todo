"use client";

export default function TodoFormCreate() {
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const title = formData.get("title") as string;
    const description = formData.get("description") as string;

    console.log(title, description);
  };

  return (
    <form
      className="items-center flex flex-col space-y-4"
      onSubmit={handleSubmit}
    >
      <input type="text" name="title" placeholder="Title" required />
      <input type="text" name="description" placeholder="Description" />
      <button type="submit">Create</button>
    </form>
  );
}
