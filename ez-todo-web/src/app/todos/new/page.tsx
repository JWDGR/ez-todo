import TodoFormCreate from '@/components/todo-form-create';
import ProtectedRoute from '@/components/ProtectedRoute';

export default function NewTodoPage() {
  return (
    <ProtectedRoute>
      <div className="p-8">
        <h1 className="text-2xl font-bold mb-6 text-center">New Todo</h1>
        <TodoFormCreate />
      </div>
    </ProtectedRoute>
  );
}