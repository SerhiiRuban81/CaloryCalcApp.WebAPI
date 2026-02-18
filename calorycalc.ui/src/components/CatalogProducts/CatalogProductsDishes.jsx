import { testCategoriesProducts } from "./testData";
import { useState } from "react";
import addNewProduct from '../../assets/plus-lg.svg';
import { useNavigate } from "react-router-dom";


export default function CatalogProductsDishes({ onFilter, onSearch }) {
    const [selectedCategory, setSelectedCategory] = useState('Всі');
    const [searchValue, setSearchValue] = useState('');
    const navigate = useNavigate();

    const handleSearchSubmit = (e) => {
        e.preventDefault();
        onSearch(searchValue);
    }

    const handleCategoryChange = (e) => {
        e.preventDefault();
        const value = e.target.value;
        setSelectedCategory(value);
        onFilter(value);
    }

    const handleAddClick = () => {
        navigate('/AddNewProductForm');
    }

    return (
        <>
            <div className="container bg-white py-4 w-75 rounded-2 mt-lg-4">
                <div >
                    <div>
                        <h1 className="fs-4 fw-bold text-start">Каталог продуктів та страв</h1>
                        <p className="mt-5 text-start">Значення в таблиці вказані на 100 г.</p>
                    </div>
                    <div style={{ position: 'relative', left: '20px' }} className="d-flex w-100 justify-content-around">
                        <form
                            className="w-50 d-flex" role="search"
                            onSubmit={handleSearchSubmit}>
                            <input
                                className="form-control me-2"
                                type="search"
                                placeholder="Знайти за назвою продукту/страви"
                                aria-label="Search"
                                value={searchValue}
                                onChange={(e) => setSearchValue(e.target.value)}
                            />
                            <button className="btn btn-outline-success"
                                type="submit">Search</button>
                        </form>
                        <select className="w-25 form-select"
                            aria-label="Default select example"
                            value={selectedCategory}
                            onChange={handleCategoryChange}>
                            <option value="Всі">Всі</option>
                            {
                                testCategoriesProducts.map(category => (
                                    <option key={category.id} value={category.id}>{category.name}</option>
                                ))}
                        </select>
                        <div style={{ position: 'relative', left: '-30px', top: '5px', cursor: 'pointer', boxSizing: 'border-box' }}>
                            <img onClick={handleAddClick} className="btn" src={addNewProduct} alt="add new product" />
                        </div>
                    </div>
                </div>

            </div>
        </>
    )
}