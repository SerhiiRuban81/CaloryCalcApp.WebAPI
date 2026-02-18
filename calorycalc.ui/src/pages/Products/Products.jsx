import "./products.css";
import MainPageProducts from "../../components/CatalogProducts/MainPageProducts";


export default function Products() {
  return (
    <>
      <div className="d-flex justify-content-center" id="divCatalogProductsDishes">
        <MainPageProducts />
      </div>
    </>
  );
}