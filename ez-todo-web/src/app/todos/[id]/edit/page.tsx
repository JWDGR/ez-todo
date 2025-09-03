import TodoFormEdit from "@/components/todo-form-edit";
import ProtectedRoute from "@/components/ProtectedRoute";

interface TodoEditPageProps {
  params: {
    id: string;
  };
}

export default function TodoEditPage({ params }: TodoEditPageProps) {
  const { id } = params;

  return (
    <ProtectedRoute>
      <div className="p-8 items-center flex flex-col">
        <h1>Edit ID: {id}</h1>
        <TodoFormEdit id={id} />
      </div>
    </ProtectedRoute>
  );
}
