let number = 0;

document.addEventListener("DOMContentLoaded", Render);
document.querySelector("#load-button").addEventListener("click", Render)
document.querySelector("#search-button").addEventListener("click", Search)

let msgBox = document.querySelector("#MessageBox");


function Render() {
    $.ajax({
        type: "POST",
        url: "/Motorcycle/GetNextEight",
        data: { "number": number },
        success: function (response) {
            number += 8;
            RenderObjects(response);
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}

function RenderObjects(data) {
    let mainBlock = document.querySelector("#main-block")
    for (let el of data) {
        mainBlock.appendChild(CreateElement(el));
    }

    function CreateElement(element) {
        let div = Object.assign(document.createElement('div'), {
            classList: 'my-1 px-1 w-full md:w-1/2 lg:my-4 lg:px-4 lg:w-1/4'
        });

        let firstSubDiv = Object.assign(document.createElement('div'), {
            classList: 'block overflow-hidden rounded-lg shadow-lg border-2 border-gray-300',
        });

        let a = Object.assign(document.createElement('div'), {
            
        });

        let img = Object.assign(document.createElement('img'), {
            classList: 'block h-auto w-full',
            alt: 'The photo should be here o_o',
            src: `${element.photo == "" ? 'images/Silhouette.jpg' : element.photo}`,
        });

        let header = Object.assign(document.createElement('header'), {
            classList: 'flex items-center justify-between leading-tight p-2 md:p-4'
        });

        let footer = Object.assign(document.createElement('footer'), {
            classList: 'flex flex-col leading-none p-2 md:p-4'
        });

        let h1 = Object.assign(document.createElement('h1'), {
            classList: 'text-2xl text-gray-600 font-medium',
            innerHTML: `${element.brand} ${element.model} `
        });

        let p1 = Object.assign(document.createElement('p'), {
            classList: 'ml-1 text-xl text-gray-600 font-medium flex justify-between items-center',
            innerHTML: `<i class="fa-solid fa-motorcycle"></i><span>${element.type}</span>`
        });

        let p2 = Object.assign(document.createElement('p'), {
            classList: 'ml-1 text-xl text-gray-600 font-medium flex justify-between items-center',
            innerHTML: `<i class="fa-solid fa-calendar-days"></i><span>${element.manufactured}</span>`
        });

        let p3 = Object.assign(document.createElement('p'), {
            classList: 'ml-1 text-xl text-gray-600 font-medium flex justify-between items-center',
            innerHTML: `<i class="fas fa-prescription-bottle"></i><span>${element.engineDisplacement * 1000} cm<sup>3</sup></span>`
        });

        let secondSubDiv = Object.assign(document.createElement('div'), {
            classList: 'p-2 w-full flex flex-row justify-between',
        });

        let priceSpan = Object.assign(document.createElement('span'), {
            classList: ' text-xl text-gray-600 font-medium px-1',
            innerHTML: `$${element.price}`
        });

        let buyLink = Object.assign(document.createElement('button'), {
            classList: "sm:text-center lg:px-4 md:mx-2 text-white font-semibold rounded bg-orange-600 hover:bg-orange-800",
            innerHTML: "Purchase"
        });

        buyLink.setAttribute("data-vin", element.vin);
        buyLink.addEventListener("click", HandleClick)

        div.appendChild(firstSubDiv);
        firstSubDiv.appendChild(a);
        firstSubDiv.appendChild(secondSubDiv);

        a.appendChild(img);
        a.appendChild(header);
        a.appendChild(footer);

        header.appendChild(h1);

        footer.appendChild(p1);
        footer.appendChild(p2);
        footer.appendChild(p3);

        secondSubDiv.appendChild(priceSpan)
        secondSubDiv.appendChild(buyLink)

        return div;
    }
}

function Search() {
    let value = document.querySelector("#search-input").value;

    $.ajax({
        type: "POST",
        url: "/Motorcycle/Search",
        data: { "value": value },
        success: function (response) {
            document.querySelector("#main-block").innerHTML = "";
            console.log(response)
            RenderObjects(response);
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}

function HandleClick(e)
{
    let element = e.currentTarget
    $.ajax({
        type: "POST",
        url: "/Order/Create",
        data: { "vin": element.dataset.vin },
        success: function (response) {
            if (response == false)
            {
                msgBox.classList.add("hidden");
            }
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}