let buttons = document.getElementsByTagName("button");

for (let i = 0; i < buttons.length; i++)
{
    buttons[i].addEventListener("click", deleteOrder)
}

function deleteOrder(e)
{
    let element = e.currentTarget
    let table = document.querySelector("#orders-table");
    $.ajax({
        type: "POST",
        url: "/Order/DeleteOrder",
        data: { "id": element.dataset.id },
        success: function (response) {
            if(response == true)
                table.removeChild(element.parentElement.parentElement)
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}

let confirmButton = document.querySelector("#buy-button")

confirmButton.addEventListener("click", ConfirmOrders)


function ConfirmOrders()
{
    let table = document.querySelector("#orders-table");
    $.ajax({
        type: "POST",
        url: "/Order/Confirm",
        success: function (response) {
            if(response == true)
                table.innerHTML = `<tr class="border-b border-gray-200">
                           
                            <td class="px-6 py-4 whitespace-no-wrap text-sm leading-5 text-gray-500"> 
                            </td>
                            <td class="px-6 py-4 whitespace-no-wrap text-sm leading-5 text-gray-500"> 
                            </td>
                            <td class="px-6 py-4 whitespace-no-wrap text-sm leading-5 text-gray-500"> 
                            </td>
                            <td class="px-6 py-4 whitespace-no-wrap text-sm leading-5 text-gray-500">
                            </td>
                            <td class="px-6 py-4 whitespace-no-wrap">
                                <div class="px-4 inline-flex text-xs leading-5 font-semibold rounded-full bg-green-200 text-green-900 mx-3 hover:cursor-pointer" id="buy-button">
                                    Buy
                                </div>
                            </td>
                        </tr>`;
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}

