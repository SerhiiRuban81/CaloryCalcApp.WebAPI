import Pagination from "../Pagination/Pagination";
import { useEffect, useState } from "react";
import TableProductsCalories from "./TableProductsCalories";
import CatalogProductsDishes from "./CatalogProductsDishes";
import { testProducts } from "./testData";


export default function MainPageProducts() {
    const [filteredProducts, setFilteredProducts] = useState(testProducts);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(2);
    const itemsPerPage = 2;

    useEffect(() => {
        setTotalPages(Math.ceil(filteredProducts.length / itemsPerPage));
    }, [filteredProducts]);

    const handleFilter = (categoryId) => {
        if (categoryId === "Всі") {
            setFilteredProducts(testProducts);
        } else {
            setFilteredProducts(
                testProducts.filter((p) => p.categoryId === Number(categoryId))
            );
        }
        setCurrentPage(1);
    };

    const handleSearch = (searchValue) => {
        if (!searchValue) {
            setFilteredProducts(testProducts);
        } else {
            const lowercasedValue = searchValue.toLowerCase();
            setFilteredProducts(
                testProducts.filter((p) =>
                    p.name.toLowerCase().includes(lowercasedValue)
                )
            );
        }
    };

    return (
        <>
            <div className="d-flex flex-column align-items-center w-100">
                <CatalogProductsDishes onFilter={handleFilter} onSearch={handleSearch} />
                <TableProductsCalories products={filteredProducts} />
                <Pagination
                    currentPage={currentPage}
                    totalPages={totalPages}
                    onPageChange={(page) => setCurrentPage(page)}
                />
            </div>
        </>
    )
}