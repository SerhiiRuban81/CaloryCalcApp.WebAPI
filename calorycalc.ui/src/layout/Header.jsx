import { NavLink } from "react-router-dom";
import icon1 from "../assets/icon1.png";
import icon2 from "../assets/icon2.png";
import icon3 from "../assets/icon3.png";
import iconMain from "../assets/iconMain.png";
import "./header.css";


export default function Header() {
    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <NavLink to="/Home" className="navbar-brand">
                    <img src={iconMain} alt="home" id="iconMain" />
                    Калькулятор Калорій
                </NavLink>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarNavAltMarkup"
                    aria-controls="navbarNavAltMarkup"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse justify-content-end" id="navbarNavAltMarkup">
                    <div className="navbar-nav">
                        <NavLink
                            to="/Products"
                            className={({ isActive }) => "nav-link btn btnHeader me-2" + (isActive ? " active" : "")}
                        >
                            <img src={icon1} alt="products" style={{ width: 20, height: 20, marginRight: 5 }} />
                            Продукти
                        </NavLink>
                        <NavLink
                            to="/Recipes"
                            className={({ isActive }) => "nav-link btn btnHeader me-2" + (isActive ? " active" : "")}
                        >
                            <img src={icon2} alt="recipes" style={{ width: 20, height: 20, marginRight: 5 }} />
                            Рецепти
                        </NavLink>
                        <NavLink
                            to="/Activity"
                            className={({ isActive }) => "nav-link btn btnHeader me-2" + (isActive ? " active" : "")}
                        >
                            <img src={icon3} alt="activity" style={{ width: 20, height: 20, marginRight: 5 }} />
                            Активність
                        </NavLink>
                         <NavLink
                            to="/Login"
                            className={({ isActive }) => "nav-link btn btnHeader me-2" + (isActive ? " active" : "")}
                        >
                            Вхід
                        </NavLink>
                    </div>
                </div>
            </div>
        </nav>
    );
}
