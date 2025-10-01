export default function RecordTheWeight({isOpen}) {

    return (
        <>
            <div className="d-flex flex-column w-25 text-center">
                <button style={{
                    backgroundColor: "#f0af37ff",
                    borderRadius: "50px",
                    width: "70px",
                    height: "70px"

                }} className="btn">
                    <img style={{
                        width: "100%",
                        height: "100%",
                    }}
                        src="/images/weight-scale_17534006.png" alt="record food"></img>
                </button>
                <p style={{
                    padding: "2px",
                    position: "relative",
                    left: isOpen? "-25px" : "-60px",
                }}>Записати вагу
                </p>
            </div>
        </>
    )
}