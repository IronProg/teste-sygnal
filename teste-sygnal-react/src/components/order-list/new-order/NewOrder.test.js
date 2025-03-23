import {render, screen} from '@testing-library/react';
import NewOrder from "./NewOrder";
import userEvent from "@testing-library/user-event";

const mockOrders = [
    [
        {
            "controlNumber": 1,
            "state": "completed"
        },
        {
            "controlNumber": 2,
            "state": "completed"
        },
        {
            "controlNumber": 3,
            "state": "inprogress"
        },
        {
            "controlNumber": 4,
            "state": "inprogress"
        },
        {
            "controlNumber": 5,
            "state": "pending"
        },
        {
            "controlNumber": 6,
            "state": "pending"
        },
    ]
]


test('new order button works', async () => {
    const addFn = jest.fn();

    render(
        <NewOrder
            onAddOrder={addFn} />
    );

    const addButton = screen.getByAltText(/plus icon/i);
    expect(addButton).toBeInTheDocument();

    userEvent.click(addButton);

    expect(addFn).toBeCalledTimes(1);
});
