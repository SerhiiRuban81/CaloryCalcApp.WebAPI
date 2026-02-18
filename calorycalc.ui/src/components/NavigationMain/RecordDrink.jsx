export default function RecordDrink({isOpen}) {

    return (
        <>
            <div className="d-flex flex-column w-25 text-center">
                <button style={{
                    backgroundColor: "#68c1dfff",
                    borderRadius: "50px",
                    width: "70px",
                    height: "70px"

                }} className="btn">
                    <img style={{
                        width: "100%",
                        height: "100%",
                    }}
                        src="/images/droplet-half.svg" alt="record food"></img>
                </button>
                <p style={{
                    padding: "2px",
                    position: "relative",
                    left: isOpen? "-25px" : "-60px",
                }}>Записати напій</p>
            </div>
        </>
    )
}