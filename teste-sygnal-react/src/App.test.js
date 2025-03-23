
import App from "./App";
import * as root from "@testing-library/react";
import {act} from "react";
import ReactDOMClient from "react-dom/client";
import {screen} from "@testing-library/react";
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
        "state": "inprogress"
    },
    {
        "controlNumber": 4,
        "state": "pending"
    },
    {
        "controlNumber": 5,
        "state": "pending"
    },
    {
        "controlNumber": 6,
        "state": "pending"
    }
]

it ('is filtering working', async () => {
    const container = document.createElement('div');
    document.body.appendChild(container);

    await act(() => {
        ReactDOMClient.createRoot(container).render(<App mockData={mockOrders} />);
    })

    const controlNumberMinimum = screen.getByLabelText(/minimum value/i);
    const controlNumberMaximum = screen.getByLabelText(/maximum value/i);
    const orderState = screen.getByLabelText(/order state/i);
    const applyFilterButton = screen.getByText(/apply filter/i);

    const allOrders = screen.getAllByTestId(/order-[0-9]+/i);
    expect(allOrders).toHaveLength(6);

    userEvent.selectOptions(orderState, "pending")
    userEvent.click(applyFilterButton);
    let filteredOrders = screen.getAllByTestId(/order-[0-9]+/i);
    expect(filteredOrders).toHaveLength(3);

    userEvent.selectOptions(orderState, "inprogress")
    userEvent.click(applyFilterButton);
    filteredOrders = screen.getAllByTestId(/order-[0-9]+/i);
    expect(filteredOrders).toHaveLength(2);

    userEvent.selectOptions(orderState, "completed")
    userEvent.click(applyFilterButton);
    filteredOrders = screen.getAllByTestId(/order-[0-9]+/i);
    expect(filteredOrders).toHaveLength(1);

    userEvent.selectOptions(orderState, "")
    userEvent.type(controlNumberMinimum, "2")
    userEvent.type(controlNumberMaximum, "4")
    userEvent.click(applyFilterButton);
    filteredOrders = screen.getAllByTestId(/order-[0-9]+/i);
    expect(filteredOrders).toHaveLength(3);
});