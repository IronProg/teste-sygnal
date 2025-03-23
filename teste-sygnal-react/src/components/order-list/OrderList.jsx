import './OrderList.css'
import OrderCard from "./order-card/OrderCard";
import NewOrder from "./new-order/NewOrder";

export default function OrderList(props) {

    return (
        <div className="order-cards-container">
            <NewOrder onAddOrder={props.onAddOrder}/>
            {
                props.orderList
                    .map(order => <OrderCard onDeleteOrder={props.onDeleteOrder} onUpdateOrder={props.onUpdateOrder}
                                             key={order.controlNumber}
                                             state={order.state}
                                             controlNumber={order.controlNumber}/>)
            }
        </div>)
}