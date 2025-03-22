import "./OrderCard.css"

export default function OrderCard(props) {
    return (
        <div className="card order-card" data-status={props.state}>
            <p className="order-control-number">{props.controlNumber}</p>
            <p className="order-badge">{props.state}</p>
        </div>
    )
}