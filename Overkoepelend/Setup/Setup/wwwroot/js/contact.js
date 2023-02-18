const form = document.querySelector("form");

const subject = document.getElementById("subject");
const email = document.getElementById("email");
const message = document.getElementById("message");

var ResponseKey;

form.addEventListener("submit", async (event) => {
    event.preventDefault();

    ToggleInputs();
    const element = document.getElementById("spinningcircle");
    if (element != null) element.classList.remove('hide');

    let valide = ValidateInputs();

    if (valide) {

        let response = await fetch('/api/DevContact/Validate', {
            method: 'post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Response: ResponseKey, Subject: subject.value, EmailAddress: email.value.toLowerCase(), Message: message.value })
        })

        let data = await response.json();

        alert(JSON.stringify(data))
        //todo change page based on data received

    } else {
        alert("You have not put in everything correctly");
    }
    if (element != null) element.classList.add('hide');
    ToggleInputs();
});

function ToggleInputs() {
    subject.disabled = !subject.disabled;
    email.disabled = !email.disabled;
    message.disabled = !message.disabled; 

    const submitbutton = document.getElementById("submitbutton");
    submitbutton.disabled = !submitbutton.disabled;
}

function ValidateInputs() {
    //todo show constrains on page
    if (subject.value >= 200 && subject.value <= 0) {
        return false;
    }

    let emailstring = email.value.toLowerCase();
    if (!/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/.test(emailstring) && emailstring >= 200 && emailstring <= 0) {
        return false;
    }

    if (message.value >= 600 && message.value <= 0) {
        return false;
    }

    if (ResponseKey == null) {
        return false;
    }

    return true;
}

//ReCaptcha
function GetResponseKey(UserResponseKey) {
    ResponseKey = UserResponseKey;
}

function ClearResponseKey() {
    ResponseKey = null;
}
