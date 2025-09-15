import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./pages/Home";
import Products from "./pages/Products/Products";
import Recipes from "./pages/Recipes";
import Activity from "./pages/Activity";
import Login from "./pages/Login";
import AddNewProductForm from "./components/AddProductDishes/AddNewProductForm";
import Layout from "./layout/Layout";
import ProductCart from "./pages/Products/ProductCart";
import { testProducts } from "./components/CatalogProducts/testData";


function App() {
  return (
    <Router>
      <div className="App container" style={{ maxWidth: "90%" }}>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="/Home" element={<Home />} />
            <Route path="/Products" element={<Products />} />
            <Route path="/Recipes" element={<Recipes />} />
            <Route path="/Activity" element={<Activity />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/AddNewProductForm" element={<AddNewProductForm />} />
            <Route path="/Products/ProductCart/:productId" element={<ProductCart products={testProducts} />} />
          </Route>
        </Routes>
      </div>
    </Router>
  );
}

export default App;
