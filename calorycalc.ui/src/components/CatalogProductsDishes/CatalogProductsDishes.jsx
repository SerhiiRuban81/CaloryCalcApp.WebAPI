export default function CatalogProductsDishes() {

    return (
        <>
            <div className="container bg-white py-4 w-75 rounded-2 mt-lg-4">
                <div>
                    <h1 className="fs-4 fw-bold text-start">Каталог продуктів та страв</h1>
                    <p className="mt-5 text-start">Значення в таблиці вказані на 100 г.</p>
                </div>
                <div className="d-flex w-100 justify-content-around">
                    <form className="w-50 d-flex" role="search">
                        <input className="form-control me-2" type="search" placeholder="Знайти за назвою продукту/страви" aria-label="Search" />
                        <button className="btn btn-outline-success" type="submit">Search</button>
                    </form>
                    <select className="w-25 form-select" aria-label="Default select example">
                        <option selected>Всі</option>
                        <option value="1">One</option>
                        <option value="2">Two</option>
                        <option value="3">Three</option>
                    </select>
                </div>
            </div>
        </>
    )
}