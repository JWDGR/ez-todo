import TodoFormEdit from "@/components/todo-form-edit";

interface TodoEditPageProps {
  params: {
    id: string;
  };
}

export default function TodoEditPage({ params }: TodoEditPageProps) {
  const { id } = params;
  return (
    <div className="p-8 items-center flex flex-col">
      <h1>Edit Todo {id}</h1>
      <TodoFormEdit
        todo={{
          id: Number(id),
          title: "Test",
          description: "Test",
          isCompleted: false,
          createdAt: new Date().toISOString(),
        }}
      />
    </div>
  );
}
