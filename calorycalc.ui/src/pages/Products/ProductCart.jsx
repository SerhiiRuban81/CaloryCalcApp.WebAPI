import { useParams } from "react-router-dom";

export default function ProductCart({ products }) {
    const { productId } = useParams();
    const product = products.find(p => p.id === parseInt(productId));

    return (
        <>
            <div className="container bg-white py-4 w-75 rounded-2 mt-lg-4">
                <h1 className="fs-4 fw-bold text-start">{product?.name}</h1>
                <p className="mt-5 text-start">Енергія: {product?.calories} ккал</p>
                <p className="mt-2 text-start">Білки: {product?.protein} г</p>
                <p className="mt-2 text-start">Жири: {product?.fat} г</p>
                <p className="mt-2 text-start">Вуглеводи: {product?.carbs} г</p>
            </div>
        </>
    );
}