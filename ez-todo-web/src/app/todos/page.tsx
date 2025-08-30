import TodosListing from "@/components/todos-listing";

export default function TodosPage() {
  return (
    <div className="p-8 items-center flex flex-col">
      <h1 className="text-2xl font-bold">My Todos</h1>
      <TodosListing />
    </div>
  );
}
