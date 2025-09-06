import CatalogProductsDishes from "../../components/CatalogProductsDishes/CatalogProductsDishes";
import Header from "../../layout/Header";
import "./products.css";


export default function Products() {
  return (
    <>
      <Header />
      <div className="d-flex justify-content-center" id="divCatalogProductsDishes">
        <CatalogProductsDishes />
      </div>
    </>
  );
}