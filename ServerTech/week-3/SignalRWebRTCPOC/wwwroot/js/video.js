"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var myConnectionId;

const initializeSignalR = () => {
    connection.start().then(() => { console.log("SignalR: Connected"); generateRandomUsername(); }).catch(err => console.log(err));
};

const setUsername = (username) => {
    connection.invoke("Join", username)

    $("#upperUsername").text(username);
    $('div.username').text(username);
};

const generateRandomUsername = () => {
    let username = 'User ' + Math.floor((Math.random() * 10000) + 1);
    setUsername(username);
};

connection.on('updateUserList', (userList) => {
    $("#usersLength").text(userList.length - 1);
    $('#usersdata li.user').remove();

    $.each(userList, function (index) {
        if (userList[index].username === $("#upperUsername").text()) {
            myConnectionId = userList[index].connectionId;
        } else {
            var listString = '<li class="list-group-item user" data-cid=' + userList[index].connectionId + ' data-username=' + userList[index].username + '>';
            listString += '<a href="#"><div class="username"> ' + userList[index].username + '</div>';
            $('#usersdata').append(listString);
        }
    });
});

// Hub Callback: Call Accepted
connection.on('callAccepted', (acceptingUser) => {
    console.log('SignalR: call accepted from: ' + JSON.stringify(acceptingUser) + '.  Initiating WebRTC call and offering my stream up...');

    // Callee accepted our call, so start chat

    // Set UI into call mode
    $('body').attr('data-mode', 'incall');
    $("#callstatus").text('In Call');
});

// Hub Callback: Call Declined
connection.on('callDeclined', (decliningUser, reason) => {
    console.log('SignalR: call declined from: ' + decliningUser.connectionId);

    // Let the user know that the callee declined to talk
    alert(reason);

    // Back to an idle UI
    $('body').attr('data-mode', 'idle');
    $("#callstatus").text('Idle');
});

// Hub Callback: Call Ended
connection.on('callEnded', (signalingUser, signal) => {

    // Let the user know why the server says the call is over
    alert(signal);

    // Set the UI back into idle mode
    $('body').attr('data-mode', 'idle');
    $("#callstatus").text('Idle');
});

// Hub Callback: Incoming Call
connection.on('incomingCall', (callingUser) => {
    console.log('SignalR: incoming call from: ' + JSON.stringify(callingUser));

    // Ask if we want to talk
    if (confirm(callingUser.username + ' is calling.  Do you want to chat?') == true) {
        // I want to chat
        connection.invoke('AnswerCall', true, callingUser).catch(err => console.log(err));

        // So lets go into call mode on the UI
        $('body').attr('data-mode', 'incall');
        $("#callstatus").text('In Call');
    } else {
        // Go away, I don't want to chat with you
        connection.invoke('AnswerCall', false, callingUser).catch(err => console.log(err));
    }
});

$(document).ready(function
    () {
    initializeSignalR();

    //Get mediadevices
    try {
        const stream = openMediaDevices({ 'video': true, 'audio': true });
        console.log('Got MediaStream:', stream);
    } catch (error) {
        console.error('Error accessing media devices.', error);
    }

   //Check media devices
    const videoCameras = getConnectedDevices('videoinput');
    console.log('Cameras found:', videoCameras);

    playVideoFromCamera();

    // Add click handler to users in the "Users" pane
    $(document).on('click', '.user', function () {
        console.log('calling user... ');
        // Find the target user's SignalR client id
        var targetConnectionId = $(this).attr('data-cid');

        connection.invoke('callUser', { "connectionId": targetConnectionId });

        // UI in calling mode
        $('body').attr('data-mode', 'calling');
        $("#callstatus").text('Calling...');
    });

    // Add handler for the hangup button
    $('.hangup').click(function () {
        console.log('hangup....');
        // Only allow hangup if we are not idle
        //localStream.getTracks().forEach(track => track.stop());
        if ($('body').attr("data-mode") !== "idle") {
            connection.invoke('hangUp');
            $('body').attr('data-mode', 'idle');
            $("#callstatus").text('Idle');
        }
    });
});

//Chat
connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

document.getElementById("sendButton").addEventListener("click", function (event) {

    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", myConnectionId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//Media
const openMediaDevices = async (constraints) => {
    return await navigator.mediaDevices.getUserMedia(constraints);
}

async function getConnectedDevices(type) {
    const devices = await navigator.mediaDevices.enumerateDevices();
    return devices.filter(device => device.kind === type)
}

// Updates the select element with the provided set of cameras
function updateCameraList(cameras) {
    const listElement = document.querySelector('select#availableCameras');
    listElement.innerHTML = '';
    cameras.map(camera => {
        const cameraOption = document.createElement('option');
        cameraOption.label = camera.label;
        cameraOption.value = camera.deviceId;
    }).forEach(cameraOption => listElement.add(cameraOption));
}

// Fetch an array of devices of a certain type
async function getConnectedDevices(type) {
    const devices = await navigator.mediaDevices.enumerateDevices();
    return devices.filter(device => device.kind === type)
}

async function playVideoFromCamera() {
    try {
        const constraints = { 'video': true, 'audio': true };
        const stream = await navigator.mediaDevices.getUserMedia(constraints);
        const videoElement = document.querySelector('video#localVideo');
        videoElement.srcObject = stream;
    } catch (error) {
        console.error('Error opening video camera.', error);
    }
}