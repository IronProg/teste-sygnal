import "./OrderCard.css"

export default function OrderCard(props) {
    const isClickable = props.state !== "completed";
    const classes = isClickable ? "card order-card clickable" : "card order-card";

    const stateString = props.state === "pending"
        ? "Pending"
        : props.state === "inprogress"
            ? "In Progress"
            : "Completed";
    return (
        <div onClick={() => isClickable ? props.onUpdateOrder(props.controlNumber) : null} className={classes}
             data-status={props.state}>
            <p className="order-control-number">{props.controlNumber}</p>
            <p className="order-badge">{stateString}</p>
        </div>
    )
}