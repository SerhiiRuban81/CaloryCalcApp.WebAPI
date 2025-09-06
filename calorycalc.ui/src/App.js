import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./pages/Home";
import Products from "./pages/Products/Products";
import Recipes from "./pages/Recipes";
import Activity from "./pages/Activity";
import Login from "./pages/Login";


function App() {
  return (
    <Router>
      <div className="App container" style={{ maxWidth: "90%" }}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/Home" element={<Home />} />
          <Route path="/Products" element={<Products />} />
          <Route path="/Recipes" element={<Recipes />} />
          <Route path="/Activity" element={<Activity />} />
          <Route path="/Login" element={<Login />} />

        </Routes>
      </div>
    </Router>
  );
}

export default App;
