import "./OrderCard.css"

export default function OrderCard(props) {
    const isClickable = props.state !== "completed";
    const isDeletable = props.state === "pending";
    const classes = isClickable ? "card order-card clickable group" : "card order-card";

    const stateString = props.state === "pending"
        ? "Pending"
        : props.state === "inprogress"
            ? "In Progress"
            : "Completed";

    function deleteOrder(e) {
        e.stopPropagation();
        props.onDeleteOrder(props.controlNumber);
    }
    return (
        <div data-testid={"order-" + props.controlNumber} onClick={() => isClickable ? props.onUpdateOrder(props.controlNumber) : null} className={classes}
             data-status={props.state}>
            {isDeletable
                ? <button onClick={deleteOrder} className='btn-delete '>
                    <img alt="X icon" src="/x-symbol-svgrepo-com.svg"/>
                </button>
                : <></>
            }
            <p className="order-control-number">{props.controlNumber}</p>
            <p className="order-badge">{stateString}</p>
        </div>
    )
}