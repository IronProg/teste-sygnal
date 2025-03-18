const $formFilter = $("#form-filter");
const $btnAddOrder = $("#btn-add-order");
const $tableContainer = $("#table-container");

$(function () {
    $formFilter.on("submit", formFilterSubmitHandler)
    $btnAddOrder.on("click", btnAddOrderClickHandler)
})


async function formFilterSubmitHandler(e) {
    e.preventDefault();
    try {
        const response = await $.ajax({
            url: `api/order?${$(this).serialize()}`,
            method: "GET",
        });

        $tableContainer.html(response);

        const $tableRows = $tableContainer.find("tbody tr");

        console.log($tableRows);
        $tableRows.find(".btn-update-order").on("click", btnUpdateOrderClickHandler);
        $tableRows.find(".btn-delete-order").on("click", btnDeleteOrderClickHandler);

        return false;
    } catch (error) {
        console.log(error);
        return false;
    }

}

async function btnAddOrderClickHandler() {
    try {
        const response = await $.ajax({
            url: `api/order?${$(this).serialize()}`,
            method: "Post",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Content-Type","application/json");
            }
        })

        $tableContainer.find("tbody").append(response);
        
        const $appendedRow = $tableContainer.find("tbody tr:last-of-type");
        
        console.log($appendedRow);
        $appendedRow.find(".btn-update-order").on("click", btnUpdateOrderClickHandler);
        $appendedRow.find(".btn-delete-order").on("click", btnDeleteOrderClickHandler);

    } catch (error) {
        console.log(error);
    }
}

async function btnDeleteOrderClickHandler() {
    try {
        const $tableRow = $(this).parents('tr');
        const controlNumber = $tableRow.attr('data-control-number');
    
        const response = await $.ajax({
            url: `api/order/${controlNumber}`,
            method: "DELETE",
        });
        
        $tableRow.remove();
    } catch (error) {
        console.log(error);
    }
}
async function btnUpdateOrderClickHandler() {
    try {
        const $tableRow = $(this).parents('tr');
        const controlNumber = $tableRow.attr('data-control-number');

        const response = await $.ajax({
            url: `api/order/${controlNumber}`,
            method: "PUT",
        });
        
        console.log(response);
        
        const $badge = $tableRow.find(".badge");
        
        $badge.text(response.stateName);
        $badge.attr("class", `badge bg-${response.stateColor}`);

        $tableRow.find(".btn-delete-order").prop("disabled", true);
        
        if (response.state === "completed")
            $(this).prop("disabled", true);
    } catch (error) {
        console.log(error);
    }
}