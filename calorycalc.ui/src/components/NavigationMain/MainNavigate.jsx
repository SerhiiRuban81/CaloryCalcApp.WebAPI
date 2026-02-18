import RecordActivity from "./RecordActivity";
import RecordDrink from "./RecordDrink";
import RecordFood from "./RecordFood";
import RecordTheWeight from "./RecordTheWeight";

export default function MainNavigate({ isOpen }) {
    return (
        <div
            style={{
                position: "absolute",
                top: "188px",
                left: isOpen ? "38vw" : "270px",
                transition: "all 0.3s ease",
                width: isOpen ? "40%" : "60%",
                boxShadow: "0px 0px 26px -5px rgba(65, 77, 218, 1)",
                display: "flex",
                justifyContent: "space-between",
                padding: "0 0 0 80px",
            }}
            className={`container bg-white py-4 rounded-2 mt-lg-4`}
        >
            <RecordFood isOpen={isOpen}/>
            <RecordDrink isOpen={isOpen}/>
            <RecordActivity isOpen={isOpen}/>
            <RecordTheWeight isOpen={isOpen}/>
        </div>
    );
}
