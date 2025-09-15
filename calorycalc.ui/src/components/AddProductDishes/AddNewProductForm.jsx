import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";


export default function AddNewProductForm({ products }) {
    const navigate = useNavigate();

    const { register, handleSubmit } = useForm({
        defaultValues: {
            nameProduct: "",
            energyKkal: "",
            fat: "",
            carbohydrates: "",
            proteins: ""
        }
    });
    const onSubmit = (data) => {
        console.log(data, "збережено");
        navigate('/Products');
    }

    const handleCloseClick = () => {
        navigate('/Products');
    }

    return (
        <div style={{ minHeight: "50vh", marginBottom: "40px" }}
            className="container-sm bg-white py-4 w-50 rounded-4 mt-lg-4 shadow">
            <form onSubmit={handleSubmit(onSubmit)}>
                <h1 className="fs-4 fw-bold text-center mb-4">Додати новий продукт</h1>
                <div className="mb-3 border-bottom pb-3 text-start">
                    <div>
                        <label className="form-label">Назва продукту</label>
                        <input
                            {...register(`nameProduct`, { required: true })}
                            className="form-control mb-2"
                        />
                    </div>
                    <div>
                        <label className="form-label">Харчова цінність на 100 г продукту</label>
                        <input
                            {...register(`energyKkal`, { required: true })}
                            className="form-control mb-2"
                        />
                    </div>
                    <div>
                        <label className="form-label">Жири на 100 г продукту</label>
                        <input
                            {...register(`fat`, { required: true })}
                            className="form-control mb-2"
                        />
                    </div>
                    <div>
                        <label className="form-label">Білки на 100 г продукту</label>
                        <input
                            {...register(`proteins`, { required: true })}
                            className="form-control mb-2"
                        />
                    </div>
                    <div>
                        <label className="form-label">Вуглеводи на 100 г продукту</label>
                        <input
                            {...register(`carbohydrates`, { required: true })}
                            className="form-control mb-2"
                        />
                    </div>
                </div>
                <div className="d-flex justify-content-between">
                    <div className="mt-3 px-3 "
                    >
                        <button style={{
                            backgroundColor: "#c4f8c4",
                            borderRadius: "10px",
                        }} type="submit"
                            className="btn">Записати</button>
                    </div>
                    <div className="mt-3 px-3 ">
                        <button style={{
                            backgroundColor: "#ece9e9ff",
                            borderRadius: "10px"
                        }} type="button"
                            onClick={handleCloseClick}
                            className="btn ">Закрити</button>
                    </div>
                </div>
            </form>
        </div>
    );
}