let brandSelect;

window.onload = (evt) =>
{
    brandSelect = document.querySelector("#brand-select");

    $.ajax({
        type: "POST",
        url: "/Brand/GetBrandsList",
        success: function (response) {
            let selectedId = brandSelect.dataset.selected;
            brandSelect.innerHTML = "";

            for (let el of response) {
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.brandName}`,
                    selected: (el.id == selectedId) ? "selected": "",
                });
                brandSelect.appendChild(option);
            }

        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}