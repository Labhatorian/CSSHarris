"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var myConnectionId;
var currentRoomId;

const initializeSignalR = () => {
    connection.start().then(() => { console.log("SignalR: Connected"); generateRandomUsername(); }).catch(err => console.log(err));
};

const setUsername = (username) => {
    connection.invoke("Join", username)

    $("#upperUsername").text(username);
};

const generateRandomUsername = () => {
    let username = 'User ' + Math.floor((Math.random() * 10000) + 1);
    setUsername(username);
};

connection.on('updateUserList', (userList) => {
    $("#usersLength").text(userList.length-1);
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

connection.on('updateRoomList', (roomList) => {
    $("#roomsLength").text(roomList.length);
    $('#usersdata li.room').remove();

    $.each(roomList, function (index) {
        var listString = '<li class="list-group-item room" data-rid=' + roomList[index].id + '>';
        listString += '<a href="#"><div class="title"> ' + roomList[index].title + '</div>';
        $('#roomsdata').append(listString);
    });
});

// Hub Callback: Room joined
connection.on('roomJoined', (RoomTitle) => {
    console.log('Room joined');

    // Set UI into call mode
    $('body').attr('data-mode', 'incall');
    $("#callstatus").text('Joined: ' + RoomTitle);
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
    if(confirm(callingUser.username + ' is calling.  Do you want to chat?') == true) {
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

$(document).ready(function () {
    initializeSignalR();

    // Add click handler to rooms in the "Rooms" pane
    $(document).on('click', '.room', function () {
        console.log('Joining room ');
        var targetRoomId = $(this).attr('data-rid');

        connection.invoke('joinRoom', targetRoomId);
        currentRoomId = targetRoomId;

        // UI in joining mode
        $('body').attr('data-mode', 'calling');
        $("#callstatus").text('Joining...');
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

    //Add handler for create room button
    $('.createroom').click(function () {
        var title = prompt("Hoe moet de kamer heten?");
        console.log('Creating room...');
        connection.invoke('createRoom', title);
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
    connection.invoke("SendMessage", currentRoomId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});