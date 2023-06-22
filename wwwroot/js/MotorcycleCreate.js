let brandSelect, modelSelect, typeSelect, engineSelect;

window.onload = (evt) => {
    engineSelect = document.querySelector("#engine-disp-select");
    brandSelect = document.querySelector("#brand-select");
    modelSelect = document.querySelector("#model-select");
    typeSelect = document.querySelector("#type-select");

    brandSelect.addEventListener("change", SelectChanged);
    let id;

    for (let i = 50; i <= 1000; i += 50) {
        let engineDisp = Object.assign(document.createElement('option'), {
            value: i / 1000,
            innerHTML: `${i} CC`
        });
        engineSelect.appendChild(engineDisp);
    }

    $.ajax({
        type: "POST",
        url: "/Brand/GetBrandsList",
        success: function (response) {
            brandSelect.innerHTML = "";

            for (let el of response) {
                if (!id)
                {
                    id = el.id;
                    RenderModels(id);
                }
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.brandName}`,
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
            typeSelect.innerHTML = "";

            for (let el of response) {
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.typeName}`
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

function RenderModels(brandId)
{
    $.ajax({
        type: "POST",
        url: "/Model/GetModelsByBrand",
        data: { "brandId": brandId},
        success: function (response) {
            modelSelect.innerHTML = "";

            for (let el of response) {
                let option = Object.assign(document.createElement('option'), {
                    value: el.id,
                    innerHTML: `${el.modelName}`
                });
                modelSelect.appendChild(option);
            }
        },
        failure: function (response) {
            console.log("Failure");
        }
    });
}