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

$(document).ready(function () {
    initializeSignalR();

    // Add click handler to rooms in the "Rooms" pane
    $(document).on('click', '.room', function () {
        console.log('Joining room ');
        var targetRoomId = $(this).attr('data-rid');

        connection.invoke('joinRoom', targetRoomId);
        currentRoomId = targetRoomId;
        $('#messageslist').empty();

        // UI in joining mode
        $('body').attr('data-mode', 'calling');
        $("#callstatus").text('Joining...');
    });

    // Add handler for the hangup button
    $('.hangup').click(function () {
        console.log('hangup....');
        currentRoomId = "";
        if ($('body').attr("data-mode") !== "idle") {
            connection.invoke('leaveRoom');
            $('body').attr('data-mode', 'idle');
            $("#callstatus").text('Idle');
            $("#leavebutton").addClass('hide');
            $("#deletebutton").addClass('hide');
            $("#createbutton").removeClass('hide');
            $('#messagesList').empty();
        }
    });

    //Add handler for create room button
    $('.createroom').click(function () {
        var title = prompt("Hoe moet de kamer heten?");
        console.log('Creating room...');
        connection.invoke('createRoom', title);
    });

    // Add handler for the hangup button
    $('.delete').click(function () {
        console.log('deleting....');

        if ($('body').attr("data-mode") !== "idle") {
            connection.invoke('deleteRoom', currentRoomId);
            currentRoomId = "";
            $('body').attr('data-mode', 'idle');
            $("#callstatus").text('Idle');
            $("#leavebutton").addClass('hide');
            $("#deletebutton").addClass('hide');
            $("#createbutton").removeClass('hide');
            $('#messagesList').empty();
        }
    });
});

connection.on('updateUserList', (userList) => {   
    $('#usersdata li.user').remove();
    if (userList === null) {
        $("#usersLength").text(0);
        return;
    }

    $("#usersLength").text(userList.length - 1);

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
    $('#roomsdata li.room').remove();

    $.each(roomList, function (index) {
        if (roomList[index].id != currentRoomId) {
            var listString = '<li class="list-group-item room" data-rid=' + roomList[index].id + '>';
            listString += '<a href="#"><div class="title"> ' + roomList[index].title + '</div>';
            $('#roomsdata').append(listString);
        }
    });
});

// Hub Callback: Room joined
connection.on('roomJoined', (RoomTitle, IsOwner, Messages) => {
    console.log('Room joined');

    // Set UI into call mode
    $('body').attr('data-mode', 'incall');
    $("#callstatus").text('Joined: ' + RoomTitle);

    //Enable leave button
    $("#leavebutton").removeClass('hide');

    //disable create button
    $("#createbutton").addClass('hide');

    //Enable delete button
    if (IsOwner) $("#deletebutton").removeClass('hide')

    Messages.forEach(message => CreateMessage(message.username, message.content));
});

// Hub Callback: Room Deleted
connection.on('roomDeleted', () => {
    console.log('Room is being deleted...');
    currentRoomId = "";
    alert("Room has been deleted :(");
    if ($('body').attr("data-mode") !== "idle") {
        $('body').attr('data-mode', 'idle');
        $("#callstatus").text('Idle');
        $("#leavebutton").addClass('hide');
        $("#createbutton").removeClass('hide');
        $('#messagesList').empty();
    }
});

//Chat
connection.on("ReceiveMessage", function (user, message) {
    CreateMessage(user, message);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", currentRoomId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function CreateMessage(user, message) {
    var li = document.createElement("li");
    var list = document.getElementById("messagesList");
    var chat = document.getElementById("chatpane");
    list.appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
    chat.scrollTop = chat.scrollHeight - chat.clientHeight;
}
