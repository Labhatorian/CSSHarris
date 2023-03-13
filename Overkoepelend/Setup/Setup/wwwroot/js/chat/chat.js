//TODO convert o module
import { User } from "./components/user.js";
import { ChatPane } from "./maincomponents/chatpane.js";
import { ChatList } from "./maincomponents/listcomponent.js";

customElements.define('chat-pane', ChatPane);
customElements.define('chat-list', ChatList);

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var myUsername;
var currentRoomId = "";

const initializeSignalR = () => {
    connection.start().then(() => { console.log("SignalR: Connected"); generateRandomUsername(); }).catch(err => console.log(err));
};

const setUsername = (username) => {
    connection.invoke("Join", username)
    myUsername = username;
    document.querySelector("chat-list[type = 'user']").setUser(username);
};

const generateRandomUsername = () => {
    let username = 'User ' + Math.floor((Math.random() * 10000) + 1);
    setUsername(username);
};

$(document).ready(function () {
    initializeSignalR();

    //Handler create join room
    this.addEventListener("joinroom", function (e) {
        if (currentRoomId != "") connection.invoke('leaveRoom', currentRoomId);
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

    //Handler send message
    this.addEventListener("sendmessage", function (e) {
       if(currentRoomId != '' && currentRoomId != undefined) connection.invoke("SendMessage", currentRoomId, e.message);
    });
});

// Hub Callback: Update users
connection.on('updateUserList', (userList) => {   
    document.querySelector("chat-list[type = 'user']").updateUsers(userList, myUsername);
});

// Hub Callback: Update rooms
connection.on('updateRoomList', (roomList) => {
    document.querySelector("chat-list[type = 'room']").updateRooms(roomList, currentRoomId);
});

// Hub Callback: Room joined
connection.on('roomJoined', (RoomTitle, IsOwner, Messages) => {
    console.log('Room joined');
    document.querySelector("chat-list[type = 'room']").updateButtons(currentRoomId, IsOwner, RoomTitle);
});

// Hub Callback: Room Deleted
connection.on('roomDeleted', () => {
    console.log('Room is being deleted...');
    currentRoomId = "";
    document.querySelector("chat-list[type = 'room']").DeletedRoomButtons();
    alert("Room has been deleted :(");
});

// Hub Callback: Receive message
connection.on("ReceiveMessage", function (user, message) {
    document.querySelector("chat-pane").NewMessage(user, message);
});