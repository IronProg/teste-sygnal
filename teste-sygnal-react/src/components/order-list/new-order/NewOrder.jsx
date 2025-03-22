import "./NewOrder.css"

export default function NewOrder(props) {
    return (
        <div onClick={props.onAddOrder} className="card" id="new-order-card">
            <p className="icon-add-order">
                <img alt="plus icon" src="/plus-svgrepo-com.svg" className="w-20 h-20" />
            </p>
        </div>
    )
}