import "./NewOrder.css"

export default function NewOrder(props) {
    return (
        <div onClick={props.onAddOrder} className="card" id="new-order-card">
            <p className="icon-add-order">+</p>
        </div>
    )
}