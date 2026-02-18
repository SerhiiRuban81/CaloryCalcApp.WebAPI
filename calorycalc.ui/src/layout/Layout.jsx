import { Outlet } from "react-router-dom";
import Header from "./Header";

export default function Layout() {
    return (
        <>
            <div>
                <Header />
                <main className="container mt-4">
                    <Outlet />
                </main>
            </div>

        </>
    );
}