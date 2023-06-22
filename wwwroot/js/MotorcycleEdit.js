
let brandSelect, modelSelect, typeSelect, engineSelect;

window.onload = (evt) => {
    engineSelect = document.querySelector("#engine-disp-select");
    brandSelect = document.querySelector("#brand-select");
    modelSelect = document.querySelector("#model-select");
    typeSelect = document.querySelector("#type-select");

    brandSelect.addEventListener("change", SelectChanged);
    let id, engineDispValue = parseFloat(engineSelect.dataset.enginedisp.replace(',', '.'));
    for (let i = 50; i <= 1000; i += 50) {
        let engineDisp = Object.assign(document.createElement('option'), {
            value: i / 1000,
            innerHTML: `${i} CC`,
            selected: (engineDispValue == i / 1000) ? "selected" : "",
        });
        engineSelect.appendChild(engineDisp);
    }

    $.ajax({
        type: "POST",
        url: "/Brand/GetBrandsList",
        success: function (response) {
            id = brandSelect.dataset.selected;
            brandSelect.innerHTML = "";
            RenderModels(id);

            for (let el of response) {
                
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.brandName}`,
                    selected: (el.id == id) ? "selected" : "",
                });
                brandSelect.appendChild(option);
            }

        },
        failure: function (response) {
            console.log("Failure");
        }
    });

    $.ajax({
        type: "POST",
        url: "/Type/GetTypeList",
        success: function (response) {
            let selectedType = typeSelect.dataset.selected
            typeSelect.innerHTML = "";

            for (let el of response) {
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.typeName}`,
                    selected: (el.id == selectedType) ? "selected" : "",
                });
                typeSelect.appendChild(option);
            }
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}

function SelectChanged(e) {
    RenderModels(e.currentTarget.value);
}

function RenderModels(brandId) {
    $.ajax({
        type: "POST",
        url: "/Model/GetModelsByBrand",
        data: { "brandId": brandId },
        success: function (response) {
            let selectedModel = modelSelect.dataset.selected
            modelSelect.innerHTML = "";

            for (let el of response) {
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.modelName}`,
                    selected: (el.id == selectedModel) ? "selected" : "",
                });
                modelSelect.appendChild(option);
            }
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}