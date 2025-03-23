import {render, screen} from '@testing-library/react';
import FilterForm from "./FilterForm";
import userEvent from "@testing-library/user-event";

test('renders form filters and submit sending filters', async () => {
    const clickFn = jest.fn();
    render(<FilterForm onFilter={clickFn}/>);
    const submitButton = screen.getByText(/apply filter/i);
    expect(submitButton).toBeInTheDocument();

    const controlNumberMinimum = screen.getByLabelText(/minimum value/i);
    expect(controlNumberMinimum).toBeInTheDocument();

    const controlNumberMaximum = screen.getByLabelText(/maximum value/i);
    expect(controlNumberMaximum).toBeInTheDocument();

    const orderState = screen.getByLabelText(/order state/i);
    expect(orderState).toBeInTheDocument();

    await userEvent.click(submitButton);

    expect(clickFn).toBeCalledTimes(1);
    expect(clickFn).toBeCalledWith({
        state: "",
        controlNumber: "",
        controlNumberMax: ""
    });

    userEvent.type(controlNumberMinimum, "5");
    userEvent.type(controlNumberMaximum, "10");
    userEvent.selectOptions(orderState, "pending");

    await userEvent.click(submitButton);

    expect(clickFn).toBeCalledTimes(2);
    expect(clickFn).toBeCalledWith({
        state: "pending",
        controlNumber: "5",
        controlNumberMax: "10"
    });


});
