import './FilterForm.css'
import React, {useState} from "react";

export default function FilterForm(props) {
    const [minimum, setMinimum] = useState("");
    const [maximum, setMaximum] = useState("");
    const [orderState, setOrderState] = useState("");

    function filterOrders() {
        const filter = {
            state: orderState,
            controlNumber: minimum,
            controlNumberMax: maximum
        }

        props.onFilter(filter);
    }

    return (<div className="card w-full">
            <form>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div className="flex flex-col">
                        <p className="form-label">Control Number</p>
                        <div className="flex flex-row justify-between">
                            <div className="floating-control group">
                                <input type="text" name="control-number-minimum" id="control-number-minimum"
                                       className="floating-input peer"
                                       value={minimum}
                                       onChange={(event) => setMinimum(event.target.value)}
                                       placeholder="" required/>
                                <label htmlFor="control-number-minimum"
                                       className="floating-label">Minimum Value</label>
                            </div>
                            <div className="floating-control group">
                                <input type="text" name="control-number-maximum" id="control-number-maximum"
                                       className="floating-input peer"
                                       value={maximum}
                                       onChange={(event) => setMaximum(event.target.value)}
                                       placeholder="" required/>
                                <label htmlFor="control-number-maximum"
                                       className="floating-label">Maximum Value</label>
                            </div>
                        </div>
                    </div>
                    <div className="flex flex-col">
                        <label className="form-label" htmlFor="order-state">Order State</label>
                        <div className="flex flex-row justify-between">
                            <select id="order-state"
                                    className="order-select"
                                    value={orderState}
                                    onChange={(event) => setOrderState(event.target.value)}>
                                <option value="">All</option>
                                <option value="pending">Pending</option>
                                <option value="inprogress">In Progress</option>
                                <option value="completed">Completed</option>
                            </select>
                        </div>
                    </div>
                </div>
                <div className="mt-4 flex justify-center">
                    <button type="button" className="button-submit-filters" onClick={filterOrders}>
                        Apply Filters
                    </button>
                </div>
            </form>
        </div>)
}