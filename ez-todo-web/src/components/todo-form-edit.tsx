import { Todo } from "@/models/Todo";

export default function TodoFormEdit({ todo }: { todo: Todo }) {
  return (
    <form className="items-center flex flex-col">
      <span>{todo.id}</span>
      <span>{todo.createdAt}</span>
      <label htmlFor="title">Title</label>
      <input type="text" name="title" placeholder="Title" required />
      <label htmlFor="description">{todo.description}</label>
      <input type="text" name="description" placeholder="Description" />
      <label htmlFor="isCompleted">{todo.isCompleted}</label>
      <input type="checkbox" name="isCompleted" />
      <button type="submit">Save</button>
      <button type="button">Delete</button>
    </form>
  );
}
