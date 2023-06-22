let hidePassBtn = document.querySelector("#HidePasswordBtn");
let showPassBtn = document.querySelector("#ShowPasswordBtn");
let passwordInput = document.querySelector("#PasswordInput");

hidePassBtn.addEventListener('click', HideShowPassword);
showPassBtn.addEventListener('click', HideShowPassword);

function HideShowPassword(e)
{
    (passwordInput.getAttribute("type") == "text") ?
        passwordInput.setAttribute("type", "password") :
        passwordInput.setAttribute("type", "text");

    showPassBtn.classList.toggle("hidden");
    hidePassBtn.classList.toggle("hidden");
}

