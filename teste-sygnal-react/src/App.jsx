import './App.css';
import Navbar from './components/layout/Navbar';
import FilterForm from "./components/form/FilterForm";
import OrderList from "./components/order-list/OrderList";
import {useEffect, useState} from "react";
import {ApiUrl} from "./globals";
import Spinner from "./components/spinner/Spinner";
import {toast} from "react-toastify";

function App() {
    const [orderList, setOrderList] = useState([])
    const [filteredOrderList, setFilteredOrderList] = useState([])
    const [filters, setFilters] = useState({controlNumber: "", controlNumberMax: "", state: ""})
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const newFilter = {...filters}
        setFilters(newFilter)
    }, [orderList])

    useEffect(() => {
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

    useEffect(() => {
        async function fetchData() {
            try {
                setIsLoading(true);
                const urlParams = new URLSearchParams(filters);
                const response = await fetch(ApiUrl + "order?" + urlParams.toString(), {
                    method: "GET",
                    mode: "cors"
                });

                const apiOrders = await response.json();

                let orders = [...apiOrders];
                if (filters.controlNumber && filters.controlNumber !== "") {
                    orders = orders.filter(item => item.controlNumber >= filters.controlNumber)
                }
                if (filters.controlNumberMax && filters.controlNumberMax !== "") {
                    orders = orders.filter(item => item.controlNumber <= filters.controlNumberMax)
                }
                if (filters.state && filters.state !== "") {
                    orders = orders.filter(item => item.state == filters.state);
                }
                setOrderList(orders);
            } catch (e) {
                toast.error("Loading data from server wasn't possible. " + e);
            } finally {
                setIsLoading(false);
            }
        }
        fetchData();
    }, [])

    async function addOrder() {
        try {
            const response = await fetch(ApiUrl + "order", {
                method: "POST",
                mode: "cors"
            });

            if (response.status !== 201) {
                toast.error("Bad response when adding order. " + await response.text());
                return;
            }

            const newOrder = await response.json();

            setOrderList(prevOrders => [...prevOrders, newOrder]);
        } catch (e) {
            toast.error("Error at adding order. " + e);
        }
    }

    async function updateOrder(controlNumber) {
        try {
            const response = await fetch(ApiUrl + "order/" + controlNumber, {
                method: "PUT",
                mode: "cors"
            });

            if (response.status !== 200) {
                toast.error("Bad response when updating order. " + await response.text());
                return;
            }

            const updatedOrder = await response.json();

            setOrderList(prevOrders =>
                prevOrders.map(order =>
                    order.controlNumber === updatedOrder.controlNumber ? updatedOrder : order
                )
            );
        } catch (e) {
            toast.error("Error at updating order. " + e);
        }
    }

    return (
        <>
            <Navbar/>
            <div className="content-container">
            <FilterForm onFilter={setFilters}/>
            <hr className="my-4"/>
                {isLoading
                    ? <div className="flex justify-center">
                        <Spinner/>
                    </div>
                    : <OrderList onUpdateOrder={updateOrder} onAddOrder={addOrder}
                                   orderList={filteredOrderList.toSorted((a, b) => b.controlNumber - a.controlNumber)}/>
                }
            </div>
        </>
    )
        ;
}

export default App;
