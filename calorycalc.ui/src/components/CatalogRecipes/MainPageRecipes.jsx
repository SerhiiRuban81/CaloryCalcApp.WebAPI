import Pagination from "../Pagination/Pagination";
import { useEffect, useState } from "react";
import TableRecipes from "./TableRecipes";
import CatalogRecipes from "./CatalogRecipes";
import { testRecipes } from "./testRecipesData";

export default function MainPageRecipes() {
    const [filteredRecipes, setFilteredRecipes] = useState(testRecipes);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const itemsPerPage = 3;

    useEffect(() => {
        setTotalPages(Math.ceil(filteredRecipes.length / itemsPerPage));
    }, [filteredRecipes]);

    const handleFilter = (categoryId) => {
        if (categoryId === "Всі") {
            setFilteredRecipes(testRecipes);
        } else {
            setFilteredRecipes(
                testRecipes.filter((r) => r.categoryId === Number(categoryId))
            );
        }
        setCurrentPage(1);
    };

    const handleSearch = (searchValue) => {
        if (!searchValue) {
            setFilteredRecipes(testRecipes);
        } else {
            const lowercasedValue = searchValue.toLowerCase();
            setFilteredRecipes(
                testRecipes.filter((r) =>
                    r.name.toLowerCase().includes(lowercasedValue)
                )
            );
        }
    };
    const indexOfLast = currentPage * itemsPerPage;
    const indexOfFirst = indexOfLast - itemsPerPage;
    const currentRecipes = filteredRecipes.slice(indexOfFirst, indexOfLast);

    return (
        <div className="d-flex flex-column align-items-center w-100">
            <CatalogRecipes onFilter={handleFilter} onSearch={handleSearch} />
            <TableRecipes recipes={currentRecipes} />
            <Pagination
                currentPage={currentPage}
                totalPages={totalPages}
                onPageChange={(page) => setCurrentPage(page)}
            />
        </div>
    );
}
