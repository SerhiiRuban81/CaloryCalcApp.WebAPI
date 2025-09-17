export default function TableRecipes({ recipes }) {
    return (
        <table className="table table-striped">
            <thead>
                <tr>
                    <th>Назва</th>
                    <th>Категорія</th>
                    <th>Калорії</th>
                    <th>Опис</th>
                </tr>
            </thead>
            <tbody>
                {recipes.map((r) => (
                    <tr key={r.id}>
                        <td>{r.name}</td>
                        <td>{r.categoryId}</td>
                        <td>{r.calories}</td>
                        <td>{r.description}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}