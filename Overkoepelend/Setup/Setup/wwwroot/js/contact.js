const form = document.querySelector("form");

const subject = document.getElementById("subject");
const email = document.getElementById("email");
const message = document.getElementById("message");

var ResponseKey;

form.addEventListener("submit", async (event) => {
    // Then we prevent the form from being sent by canceling the event
    //TODO check input
    event.preventDefault();

    let response = await fetch('/api/DevContact', {
        method: 'post',
        body: JSON.stringify({ Response: ResponseKey, Subject: subject.value, EmailAddress: email.value, Message: message.value }),
        contentType: 'application/json; charset=utf-8',
    })

    let data = await response.json();
    alert(JSON.stringify(data))

});

function GetResponseKey(UserResponseKey) {
    ResponseKey = UserResponseKey;
}
//TODO clear key