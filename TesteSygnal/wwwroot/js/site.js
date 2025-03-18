const $formFilter = $("#form-filter");
const $formAddOrder = $("#form-add-order");
const $tableContainer = $("#table-container");

$(function () {

    $formFilter.on("submit", formFilterSubmitHandler)
    $formAddOrder.on("submit", formAddOrderSubmitHandler)
})


async function formFilterSubmitHandler(e) {
    e.preventDefault();
    try {
        const response = await $.ajax({
            url: `api/order?${$(this).serialize()}`,
            method: "GET",
        });

        $tableContainer.html(response);

        return false;
    } catch (error) {
        console.log(error);
        return false;
    }

}

async function formAddOrderSubmitHandler(e) {
    e.preventDefault();
    const $controlNumberInput = $(this).find('input[name="controlNumber"]');
    try {
        const response = await $.ajax({
            url: `api/order?${$(this).serialize()}`,
            method: "Post",
            body: JSON.stringify({
                controlNumber: $controlNumberInput.val()
            }),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Content-Type","application/json");
            }
    })

        $tableContainer.find("table tbody").append(response);

        return false;
    } catch (error) {
        console.log(error);
        return false;
    }
}