import "./recipes.css"
import MainPageRecipes from "../../components/CatalogRecipes/MainPageRecipes";
export default function Recipes() {
    return (
        <>
            <div className="d-flex justify-content-center" id="divCatalogRecipesDishes">
                <MainPageRecipes />
            </div>
        </>
    );
}