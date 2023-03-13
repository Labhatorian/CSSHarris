//TODO convert o module
import { ChatPane } from "./maincomponents/chatpane.js";
import { ChatList } from "./maincomponents/listcomponent.js";

customElements.define('chat-pane', ChatPane);
customElements.define('chat-list', ChatList);

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

    //Handler create join room
    this.addEventListener("joinroom", function (e) {
        currentRoomId = e.roomid;
        connection.invoke('joinRoom', e.roomid);
    });

    //Handler create new room
    this.addEventListener("createroom", function (e) {
        connection.invoke('createRoom', e.newroom);
    });

    //Handler delete room as owner
    this.addEventListener("deleteroom", function (e) {
        connection.invoke('deleteRoom', currentRoomId);
        currentRoomId = "";
    });

    //Handler leave room
    this.addEventListener("leaveroom", function (e) {
        currentRoomId = "";
        connection.invoke('leaveRoom');
    });
});

connection.on('updateUserList', (userList) => {   
    document.querySelector("chat-list[type = 'room']").updateRooms(userList, myConnectionId);
});

connection.on('updateRoomList', (roomList) => {
    document.querySelector("chat-list[type = 'room']").updateRooms(roomList, currentRoomId);
});

// Hub Callback: Room joined
connection.on('roomJoined', (RoomTitle, IsOwner, Messages) => {
    console.log('Room joined');
    document.querySelector("chat-list[type = 'room']").updateButtons(currentRoomId, IsOwner, RoomTitle);
    //todo add messages to chatpane and enable sendbutton
});

// Hub Callback: Room Deleted
connection.on('roomDeleted', () => {
    console.log('Room is being deleted...');
    currentRoomId = "";
    alert("Room has been deleted :(");
    //todo update buttons, clear messages, disale sendbutton
});

//Chat
connection.on("ReceiveMessage", function (user, message) {
    //todo add webcomponent to chatpane
    CreateMessage(user, message);
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", currentRoomId, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

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