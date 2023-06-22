let buttons = document.getElementsByTagName("button");
for (let i = 0; i < buttons.length; i++)
{
    buttons[i].addEventListener("click", HandleClick);
}

function HandleClick(e) {
    let element = e.currentTarget
    $.ajax({
        type: "POST",
        url: "/Order/Create",
        data: { "vin": element.dataset.vin },
        success: function (response) {
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}