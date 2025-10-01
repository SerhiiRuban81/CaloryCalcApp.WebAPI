import { useState } from "react";
import "./leftNav.css";
import MainNavigate from "./MainNavigate";


export default function LeftNavigationPanel() {
    const [isOpen, setIsOpen] = useState(false);
    const toggleMenu = () => {
        setIsOpen(!isOpen);
    };

    return (
        <>
            <div>
                <button
                    style={{
                        border: "none",
                        backgroundColor: "transparent",
                        display: "flex",
                        position: "relative",
                        top: "50px",
                        left: "140px",
                    }}
                    onClick={toggleMenu}
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseWidthExample"
                    aria-expanded="false"
                    aria-controls="collapseWidthExample">
                    <img style={{ width: "20px" }}
                        src="/images/list.svg" alt="list" />
                    <p style={{
                        width: "200px",
                        marginLeft: "-20px",
                        marginTop: "15px",
                    }} >
                        {isOpen ? "Приховати меню" : "Переглянути меню"}
                    </p>
                    <img style={{ marginLeft: "-30px" }}
                        src="/images/caret-right-fill.svg"
                        alt="right" />
                </button>
            </div>
            <div style={{ minHeight: "120px" }}>
                <div className="collapse collapse-horizontal" id="collapseWidthExample">
                    <div style={{
                        width: "200px",
                        top: "50px",
                        position: "relative",
                        left: "11vw"
                    }}
                        className="offcanvas-body">
                        <div style={{margin: "0", padding: "0"}} className="list-group w-100">
                            <a href="#"
                                className="list-group-item list-group-item-action"
                                aria-current="true" >Раціон</a>
                            <a href="#"
                                className="list-group-item list-group-item-action">Статистика</a>
                            <a href="#"
                                className="list-group-item list-group-item-action">Налаштування</a>
                            <a href="#"
                                className="list-group-item list-group-item-action">Мої страви</a>
                        </div>
                    </div>
                </div>
            </div>
            <MainNavigate isOpen={isOpen} />
        </>);
}

