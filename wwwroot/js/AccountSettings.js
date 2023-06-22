let buttons = document.getElementsByTagName("span")

for (let i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", HandleClick)
}

function HandleClick(e) {
    let element = e.currentTarget;
    let textElement = element.parentElement.parentElement.children[2]
    $.ajax({
        type: "POST",
        url: "/Account/UpdateRole",
        data: { "login": e.currentTarget.dataset.login },
        success: function (response) {
            let classString = "px-2 inline-flex text-xs leading-5 font-semibold rounded-full";

            element.innerHTML = `Make a ${response}`;
            if (response == "Moderator") {
                textElement.innerHTML = "User";
                element.classList = classString + " bg-green-100 text-green-800";
            }
            else if (response == "User") {
                textElement.innerHTML = "Moderator";
                element.classList = classString + " bg-red-100 text-red-800";
            }
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}
