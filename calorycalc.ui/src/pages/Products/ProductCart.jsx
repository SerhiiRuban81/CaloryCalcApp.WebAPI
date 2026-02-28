import { useParams } from "react-router-dom";
import "./productcart.css";
import AddProductInMenuCart from "./AddProductInMenuCart";


export default function ProductCart({ products }) {
    const { productId } = useParams();
    const product = products.find(p => p.id === parseInt(productId));

    return (
        <>
            <div className="container bg-white py-4 w-75 rounded-2 mt-lg-4">
                <div id="divCart" className="row g-0">
                    <div className="col-md-4">
                        <img src={product?.img}
                            alt={product?.name}
                            className="img-fluid rounded-start" />
                    </div>
                    <div id="cartInfo" className="col-md-6">
                        <div className="card-body text-start">
                            <h1 className="card-title">{product?.name}</h1>
                            <p className="card-text"><strong>Енергія:</strong> {product?.calories} ккал</p>
                            <p className="card-text"><strong>Білки:</strong> {product?.protein} г</p>
                            <p className="card-text"><strong>Жири:</strong> {product?.fat} г</p>
                            <p className="card-text"><strong>Вуглеводи:</strong> {product?.carbs} г</p>
                        </div>
                        <div>
                            <AddProductInMenuCart />
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}