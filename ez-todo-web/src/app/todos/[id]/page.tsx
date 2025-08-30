interface TodoPageProps {
  params: {
    id: string;
  };
}

export default function TodoPage({ params }: TodoPageProps) {
  const { id } = params;
  return (
    <div className="p-8 items-center flex flex-col">
      <h1>ID: {id}</h1>
    </div>
  );
}
