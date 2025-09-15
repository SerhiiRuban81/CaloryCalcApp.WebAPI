import { useEffect } from "react";
import { Tooltip } from "bootstrap";
import { useNavigate } from "react-router-dom";


export default function TableProductsCalories({ products }) {
    const navigate = useNavigate();

    const handleAddProductInRationClick = () => {
        alert('Продукт додано в раціон');
    }

    useEffect(() => {
        const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
        tooltipTriggerList.forEach(
            (tooltipTriggerEl) => new Tooltip(tooltipTriggerEl)
        );
    }, [products]);

    const openCartProduct = (productId) => {
        navigate(`/Products/ProductCart/${productId}`);
    }


    return (
        <div className="container bg-white py-4 w-75 rounded-2 mt-lg-4">
            <table className="table">
                <thead className="text-start">
                    <tr>
                        <th scope="col">Назва</th>
                        <th scope="col">Енергія (ккал)</th>
                        <th scope="col"></th>
                    </tr>
                </thead>
                <tbody className="text-start align-middle">
                    {products.map(product => (
                        <tr key={product.id}>
                            <td style={{ cursor: "pointer" }}
                                onClick={() => openCartProduct(product.id)}>
                                {product.name}</td>
                            <td>{product.calories}</td>
                            <td>
                                <div>
                                    <button type="button"
                                        className="btn"
                                        data-bs-toggle="tooltip"
                                        data-bs-placement="left"
                                        data-bs-title="Додати їжу в раціон"
                                        onClick={handleAddProductInRationClick}
                                        //onClick={() => handleAddProductInRationClick(product)}
                                        style={{ cursor: "pointer", border: "none", background: "transparent", padding: 0 }}  >

                                        <svg xmlns="http://www.w3.org/2000/svg"
                                            width="25" height="25" className="bi bi-plus-circle" viewBox="0 0 16 16">
                                            <circle cx="8" cy="8" r="8" fill="#8ceb8cff" />
                                            <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4" />
                                        </svg>
                                    </button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
