const $formFilter = $("#form-filter");
const $btnAddOrder = $("#btn-add-order");
const $tableContainer = $("#table-container");
const $loader = $("#loader");
toastr.option = {
    duration: 330,
    timeOut: 2000
};

const loader = {
    show: () => $loader.css("visibility", "visible"),
    hide: () => $loader.css("visibility", "hidden")
}

$(function () {
    $formFilter.on("submit", formFilterSubmitHandler)
    $btnAddOrder.on("click", btnAddOrderClickHandler)
})


async function formFilterSubmitHandler(e) {
    e.preventDefault();
    try {
        loader.show();
        const response = await $.ajax({
            url: `api/order?${$(this).serialize()}`,
            method: "GET",
        });

        $tableContainer.html(response);

        const $tableRows = $tableContainer.find("tbody tr");

        $tableRows.find(".btn-update-order").on("click", btnUpdateOrderClickHandler);
        $tableRows.find(".btn-delete-order").on("click", btnDeleteOrderClickHandler);

        return false;
    } catch (error) {
        toastr.error(error);
        return false;
    } finally {
        loader.hide();
    }
}

async function btnAddOrderClickHandler() {
    try {
        loader.show();
        const response = await $.ajax({
            url: `api/order?${$(this).serialize()}`,
            method: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Content-Type","application/json");
            }
        })

        $tableContainer.find("tbody").append(response);
        
        const $appendedRow = $tableContainer.find("tbody tr:last-of-type");
        
        $appendedRow.find(".btn-update-order").on("click", btnUpdateOrderClickHandler);
        $appendedRow.find(".btn-delete-order").on("click", btnDeleteOrderClickHandler);

        toastr.success("Order added successfully!");
    } catch (error) {
        toastr.error(error);
    } finally {
        loader.hide();
    }
}

async function btnDeleteOrderClickHandler() {
    try {
        loader.show();
        const $tableRow = $(this).parents('tr');
        const controlNumber = $tableRow.attr('data-control-number');
    
        const response = await $.ajax({
            url: `api/order/${controlNumber}`,
            method: "DELETE",
        });
        
        $tableRow.remove();
        toastr.success("Order removed successfully!");
    } catch (error) {
        toastr.error(error);
    } finally {
        loader.hide();
    }
}
async function btnUpdateOrderClickHandler() {
    try {
        loader.show();
        const $tableRow = $(this).parents('tr');
        const controlNumber = $tableRow.attr('data-control-number');

        const response = await $.ajax({
            url: `api/order/${controlNumber}`,
            method: "PUT",
        });
        
        const $badge = $tableRow.find(".badge");
        
        $badge.text(response.stateName);
        $badge.attr("class", `badge bg-${response.stateColor}`);

        $tableRow.find(".btn-delete-order").prop("disabled", true);
        
        if (response.state === "completed")
            $(this).prop("disabled", true);
        toastr.success("Order state changed to " + response.stateName);
    } catch (error) {
        toastr.error(error);
    } finally {
        loader.hide();
    }
}