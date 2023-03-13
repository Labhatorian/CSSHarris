const form = document.querySelector("form");

const subject = document.getElementById("subject");
const email = document.getElementById("email");
const message = document.getElementById("message");

var ResponseKey; //Captcha

//When submit gets pressed, send and receive data to API
form.addEventListener("submit", async (event) => {
    event.preventDefault();

    ToggleInputs();

    let valide = ValidateInputs();
    if (valide) {

        let response = await fetch('/api/DevContact/Contact', {
            method: 'post',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-Content-Type-Options': 'nosniff'
            },
            body: JSON.stringify({ Response: ResponseKey, Subject: subject.value, EmailAddress: email.value.toLowerCase(), Message: message.value })
        })

        let resp = await response;
        let data = await resp.json();
        data = await JSON.parse(data);

        if (resp.ok) {
            alert("Uw bericht is succesvol verzonden!\n Email: " + data["Email"] + "\n Onderwerp: " + data["Subject"] + "\n Bericht: " + data["Message"]);
        } else if (resp.status == 403) {
            if (data == 0) alert("Het is niet gelukt de captcha te checken");
            if (data == 1) alert("Het is niet gelukt de mail te verzenden");
            ToggleInputs();
            return;
        } else {
            alert("Er is een onbekende fout opgetreden");
        }
        window.location.reload();
    } 
    ToggleInputs();
});

//Toggle form inputs and loading circle
function ToggleInputs() {
    subject.disabled = !subject.disabled;
    email.disabled = !email.disabled;
    message.disabled = !message.disabled; 

    const submitbutton = document.getElementById("submitbutton");
    submitbutton.disabled = !submitbutton.disabled;

    //Loading circle
    const element = document.getElementById("spinningcircle");
    if (element != null && submitbutton.disabled) {
        element.classList.remove('hide');
    } else {
        element.classList.add('hide');
    }
}

//Validation with error messages
function ValidateInputs() {
    let fail = false;

    if (subject.value >= 200 && subject.value <= 0) {
        document.getElementById("ErrorSubject").classList.remove('hide');;
        fail = true;
    }

    let emailstring = email.value.toLowerCase();
    if (!/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/.test(emailstring) && emailstring >= 200 && emailstring <= 0) {
        document.getElementById("ErrorEmail").classList.remove('hide');;
        fail = true;
    }

    if (message.value >= 600 && message.value <= 0) {
        document.getElementById("ErrorMessage").classList.remove('hide');;
        fail = true;
    }

    if (ResponseKey == null) {
        document.getElementById("ErrorCaptcha").classList.remove('hide');;
        fail = true;
    }

    return !fail;
}

//ReCaptcha
function GetResponseKey(UserResponseKey) {
    ResponseKey = UserResponseKey;
}

function ClearResponseKey() {
    ResponseKey = null;
}
