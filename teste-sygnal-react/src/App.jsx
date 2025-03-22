import './App.css';
import Navbar from './components/layout/Navbar';
import FilterForm from "./components/form/FilterForm";
import OrderList from "./components/order-list/OrderList";
import {useEffect, useState} from "react";

const orderListMockup = [
    {
        "controlNumber": 2,
        "state": "completed"
    },
    {
        "controlNumber": 7,
        "state": "completed"
    },
    {
        "controlNumber": 9,
        "state": "completed"
    },
    {
        "controlNumber": 13,
        "state": "inprogress"
    },
    {
        "controlNumber": 15,
        "state": "inprogress"
    },
    {
        "controlNumber": 14,
        "state": "completed"
    },
    {
        "controlNumber": 10,
        "state": "completed"
    },
    {
        "controlNumber": 24,
        "state": "pending"
    },
    {
        "controlNumber": 8,
        "state": "completed"
    },
    {
        "controlNumber": 25,
        "state": "pending"
    },
    {
        "controlNumber": 27,
        "state": "completed"
    },
    {
        "controlNumber": 20,
        "state": "completed"
    },
    {
        "controlNumber": 28,
        "state": "pending"
    }
];

function App() {
    const [orderList, setOrderList] = useState(orderListMockup)
    const [filteredOrderList, setFilteredOrderList] = useState(orderListMockup)
    const [filters, setFilters] = useState({ controlNumber: "", controlNumberMax: "", state: ""})

    useEffect(() => {
        const newFilter = {...filters}
        setFilters(newFilter)
    }, [orderList])

    useEffect(() => {
        const urlParams = new URLSearchParams(filters);

        let orders = [...orderList];
        if (filters.controlNumber && filters.controlNumber !== "") {
            orders = orders.filter(item => item.controlNumber >= filters.controlNumber)
        }
        if (filters.controlNumberMax && filters.controlNumberMax !== "") {
            orders = orders.filter(item => item.controlNumber <= filters.controlNumberMax)
        }
        if (filters.state && filters.state !== "") {
            orders = orders.filter(item => item.state == filters.state);
        }
        setFilteredOrderList(orders);
    }, [filters])

    async function addOrder() {
        const orders = [
            ...orderList,
            { controlNumber: 33, state: "pending" }
        ];
        setOrderList(orders);
    }

    async function updateOrder(controlNumber) {
        const orders = [...orderList];
        const orderIndex = orderList.findIndex(order => order.controlNumber === controlNumber);
        const orderState = orders[orderIndex].state;

        if (orderState === 'pending') {
            orders[orderIndex].state = 'inprogress';
        } else if (orderState === 'inprogress') {
            orders[orderIndex].state = 'completed';
        }

        setOrderList(orders);
    }



    return (
        <>
            <Navbar/>
            <div className="content-container">
                <FilterForm onFilter={setFilters}/>
                <hr className="my-4" />
                <OrderList onUpdateOrder={updateOrder} onAddOrder={addOrder} orderList={filteredOrderList.toSorted((a, b) => b.controlNumber - a.controlNumber)} />
            </div>
        </>
    );
}

export default App;
