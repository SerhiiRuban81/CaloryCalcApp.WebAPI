export default function CatalogRecipes({ onFilter, onSearch }) {
    return (
        <div className="d-flex flex-column mb-3">
            <div className="mb-2">
                <select onChange={(e) => onFilter(e.target.value)}>
                    <option value="Всі">Всі</option>
                    <option value="1">Перші страви</option>
                    <option value="2">Другі страви</option>
                    <option value="3">Національні страви</option>
                </select>
            </div>
            <input
                type="text"
                placeholder="Пошук рецепта..."
                onChange={(e) => onSearch(e.target.value)}
            />
        </div>
    );
}