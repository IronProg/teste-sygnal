import {render, screen} from '@testing-library/react';
import OrderCard from "./OrderCard";
import userEvent from "@testing-library/user-event";

const mockOrders = [
    {
        "controlNumber": 1,
        "state": "completed"
    },
    {
        "controlNumber": 2,
        "state": "inprogress"
    },
    {
        "controlNumber": 3,
        "state": "pending"
    }
]


test('all order card buttons works', async () => {
    const updateFn = jest.fn();
    const deleteFn = jest.fn();

    const pendingOrder = mockOrders[2];

    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={pendingOrder.controlNumber}
            state={pendingOrder.state}/>
    );
    const orderControlNumber = screen.getByText(/[0-9]+/i);
    expect(orderControlNumber).toBeInTheDocument();

    const deleteIcon = screen.getByAltText(/x icon/i);
    expect(deleteIcon).toBeInTheDocument();

    userEvent.click(orderControlNumber);
    userEvent.click(deleteIcon);

    expect(updateFn).toBeCalledTimes(1);
    expect(updateFn).toBeCalledWith(pendingOrder.controlNumber);
    expect(deleteFn).toBeCalledTimes(1);
    expect(deleteFn).toBeCalledWith(pendingOrder.controlNumber);
});

test('order card state dont update when completed', async () => {
    const updateFn = jest.fn();
    const deleteFn = jest.fn();

    const completedOrder = mockOrders[0];
    const inProgressOrder = mockOrders[1];
    const pendingOrder = mockOrders[2];

    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={completedOrder.controlNumber}
            state={completedOrder.state}/>
    );
    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={inProgressOrder.controlNumber}
            state={inProgressOrder.state}/>
    );
    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={pendingOrder.controlNumber}
            state={pendingOrder.state}/>
    );
    const orderControlNumbers = screen.getAllByText(/[0-9]+/i);
    expect(orderControlNumbers).toHaveLength(3);

    orderControlNumbers.forEach(orderControlNumber => {
        userEvent.click(orderControlNumber);
    })

    expect(updateFn).toBeCalledTimes(2);
})

test('order card delete only work on pending', async () => {
    const updateFn = jest.fn();
    const deleteFn = jest.fn();

    const completedOrder = mockOrders[0];
    const inProgressOrder = mockOrders[1];
    const pendingOrder = mockOrders[2];

    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={completedOrder.controlNumber}
            state={completedOrder.state}/>
    );
    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={inProgressOrder.controlNumber}
            state={inProgressOrder.state}/>
    );
    render(
        <OrderCard
            onUpdateOrder={updateFn}
            onDeleteOrder={deleteFn}
            controlNumber={pendingOrder.controlNumber}
            state={pendingOrder.state}/>
    );
    const deleteIcons = screen.getAllByAltText(/x icon/i);
    expect(deleteIcons).toHaveLength(1);

    userEvent.click(deleteIcons[0]);

    expect(updateFn).toBeCalledTimes(0);
    expect(deleteFn).toBeCalledTimes(1);
})
