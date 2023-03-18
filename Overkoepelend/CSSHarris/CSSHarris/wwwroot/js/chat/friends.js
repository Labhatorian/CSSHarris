import { ChatList } from "./maincomponents/listcomponent.js";

customElements.define('chat-list', ChatList);

var connection = new signalR.HubConnectionBuilder().withUrl("/friendHub").build();

const initializeSignalR = () => {
    connection.start().then(() => { console.log("SignalR: Connected");  }).catch(err => console.log(err));
};

$(document).ready(function () {
    initializeSignalR();

    this.addEventListener("decideFriend", function (e) {
        connection.invoke('decideFriend', e.userID, e.confirm);
    });

    this.addEventListener("deleteFriend", function (e) {
        connection.invoke('deleteFriend', e.userID);
    });
});

connection.on('GetAllFriends', (requestList, friendList) => {
    document.querySelector("[type='friend'][myfriends='1']").updateFriendList(friendList, "friend");
    document.querySelector("[type='friend'][myfriends='0']").updateFriendList(requestList, "request");
});