const form = document.querySelector("form");

const subject = document.getElementById("subject");
const email = document.getElementById("email");
const message = document.getElementById("message");

var ResponseKey = "a";

form.addEventListener("submit", async (event) => {
    event.preventDefault();

   //TODO check input

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

});

function GetResponseKey(UserResponseKey) {
    ResponseKey = UserResponseKey;
}
//TODO clear key