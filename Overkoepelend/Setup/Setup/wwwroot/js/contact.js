const form = document.querySelector("form");

const subject = document.getElementById("subject");
const email = document.getElementById("email");
const message = document.getElementById("message");

var ResponseKey;

form.addEventListener("submit", async (event) => {
    event.preventDefault();

    let valide = ValidateInputs();

    if (valide) {

        let response = await fetch('/api/DevContact/Validate', {
            method: 'post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Response: ResponseKey, Subject: subject.value, EmailAddress: email.value, Message: message.value })
        })

        let data = await response.json();
        alert(JSON.stringify(data))
        //todo change page based on data received
        //todo show constrains on page
    } else {
        alert("You have not put in everything correctly");
    }
});

function ValidateInputs() {
    if (subject.value >= 200 && subject.value <= 0) {
        return false;
    }

    if (!/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/.test(email.value) && email.value >= 200 && email.value <= 0) {
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

//todo Prevent XSS

//ReCaptcha
function GetResponseKey(UserResponseKey) {
    ResponseKey = UserResponseKey;
}

function ClearResponseKey() {
    ResponseKey = null;
}