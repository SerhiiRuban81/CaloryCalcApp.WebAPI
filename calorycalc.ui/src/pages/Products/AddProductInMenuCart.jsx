import { useForm } from "react-hook-form"


export default function AddProductInMenuCart() {
    const { register, handleSubmit, watch } = useForm();

    const onSubmit = (data) => {
        alert("Продукт додано в раціон");
    }
    const selectedValue = watch("productRacion");
    const count = watch("countProduct", "100г");

    const grams = parseInt(selectedValue, 10) || 0;
    const total = grams * count;

    return (
        <form className="d-flex flex-row mb-3 gap-2 mt-4" onSubmit={handleSubmit(onSubmit)}>
            <input style={{
                width: "50px",
                height: "50px",
                borderRadius: "12px",
                border: "1px solid rgba(175, 177, 179, 0.53)",
                textAlign: "center",
            }}{...register("countProduct")}
                defaultValue={1} />
                <img src="/images/x.svg" alt="X" />
            <select style={{
                width: "100px",
                height: "50px",
                borderRadius: "12px",
                border: "1px solid  rgba(175, 177, 179, 0.53)",
                textAlign: "center",
                appearance: "none",
                background: "url('/images/caret-down.svg') no-repeat right 8px center",
                backgroundSize: "12px auto",
                paddingRight: "20px",
            }} {...register("productRacion")}>
                <option value="1г">1 г</option>
                <option value="100г">100 г</option>
                <option value="50г">50 г</option>
            </select>
            <span style={{marginTop: "10px"}}><b>= {total} г</b></span>
            <button style={{
                backgroundColor: "#8ff38fff",
                borderRadius: "12px",
                height: "50px",
            }} type="submit">Записати в раціон</button>
        </form>
    )
}