let toggleBtn = document.querySelector("#navbar-toggle");
let collapse = document.querySelector("#navbar-collapse");
let profileSpan = document.querySelector("#profileSpan");
let avatar = document.querySelector("#avatarBlock");
let messageButton = document.querySelector("#close-message");

toggleBtn.onclick = () => {
    collapse.classList.toggle("hidden");
    collapse.classList.toggle("flex");
    avatar.classList.toggle("flex");
    profileSpan.classList.toggle("hidden");
};

if (messageButton != null)
{
    messageButton.onclick = () => {
        let messageBox = document.querySelector("#Message");
        messageBox.parentElement.removeChild(messageBox);
    }
}
