"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/videoHub").build();
var peerConnection;
var myConnectionId;

const initializeSignalR = () => {
    connection.start().then(() => { console.log("SignalR: Connected"); generateRandomUsername(); }).catch(err => console.log(err));
};

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

    //Create peer connection
    //FAILED Unable to FIX, will not be done
    peerConnection = new RTCPeerConnection({
        iceServers: [
            {
                urls: "stun:relay.metered.ca:80",
            },
            {
                urls: "turn:relay.metered.ca:80",
                username: "0456",
                credential: "V",
            },
            {
                urls: "turn:relay.metered.ca:443",
                username: "06",
                credential: "/",
            },
            {
                urls: "turn:relay.metered.ca:443?transport=tcp",
                username: "",
                credential: "",
            },
        ],
    });

    // Add click handler to users in the "Users" pane
    $(document).on('click', '.user', async function () {
        console.log('calling user... ');
        // Find the target user's SignalR client id
        var targetConnectionId = $(this).attr('data-cid');

        //Create offer
        var offer = await CreateOffer();

        // Listen for connectionstatechange on the local RTCPeerConnection
        peerConnection.addEventListener('connectionstatechange', event => {
            if (peerConnection.connectionState === 'connected') {
                // Peers connected!
            }
        });

        connection.invoke('CallUserVideo', { "connectionId": targetConnectionId }, offer);
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

async function CreateOffer() {
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);

    return offer;
}

//User
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

//SignalR Handeling
// Hub Callback: Call Accepted
connection.on('callAccepted', async function(acceptingUser, answer) {
    console.log('SignalR: call accepted from: ' + JSON.stringify(acceptingUser) + '.  Initiating WebRTC call and offering my stream up...');

    // Callee accepted our call, so start chat
    await StartPeer(answer);

    //Stream remote video
    const remoteVideo = document.querySelector('#remoteVideo');

    peerConnection.addEventListener('track', async (event) => {
        const [remoteStream] = event.streams;
        remoteVideo.srcObject = remoteStream;
    });

    // Set UI into call mode
    $('body').attr('data-mode', 'incall');
    $("#callstatus").text('In Call');
});

async function StartPeer(answer) {
    const remoteDesc = new RTCSessionDescription(answer);
    await peerConnection.setRemoteDescription(remoteDesc);
}

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
connection.on('incomingCall', async function(callingUser, offer) {
    console.log('SignalR: incoming call from: ' + JSON.stringify(callingUser));

    // Ask if we want to talk
    if (confirm(callingUser.username + ' is calling.  Do you want to chat?') == true) {
        // I want to chat

        const answer = await RespondPeer(offer);
        connection.invoke('AnswerVideo', true, callingUser, answer).catch(err => console.log(err));

        //Remote video
        const remoteVideo = document.querySelector('#remoteVideo');

        peerConnection.addEventListener('track', async (event) => {
            const [remoteStream] = event.streams;
            remoteVideo.srcObject = remoteStream;
        });

        // So lets go into call mode on the UI
        $('body').attr('data-mode', 'incall');
        $("#callstatus").text('In Call');
    } else {
        // Go away, I don't want to chat with you
        connection.invoke('AnswerCall', false, callingUser).catch(err => console.log(err));
    }
});

async function RespondPeer(offer) {
    peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    return answer;
}
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

        //Send to peer
        stream.getTracks().forEach(track => {
            peerConnection.addTrack(track, stream);
        });

        const videoElement = document.querySelector('video#localVideo');
        videoElement.srcObject = stream;
    } catch (error) {
        console.error('Error opening video camera.', error);
    }
}