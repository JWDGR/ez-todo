import TodoFormCreate from '@/components/todo-form-create';

export default function NewTodoPage() {
  return (
    <div className="p-8">
      <h1 className="text-2xl font-bold mb-6 text-center">New Todo</h1>
      <TodoFormCreate />
    </div>
  );
}