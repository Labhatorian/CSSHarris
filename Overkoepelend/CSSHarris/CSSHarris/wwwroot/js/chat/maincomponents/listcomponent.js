import { RoomButtons } from "../components/buttons.js";
import { AdditionalInfo } from "../components/additionalinfo.js";

import { Room } from "../components/room.js";
import { User } from "../components/user.js";

customElements.define('room-buttons', RoomButtons);
customElements.define('additional-info', AdditionalInfo);

customElements.define('chat-room', Room);
customElements.define('chat-user', User);

const listTemplate = {
    id: 'list-tpl',
    template: `
     <section id="chatlist">
            <div class="bg-light chat-list">
            <div class="list-group-item item" id="component1">
         
            </div>

            <ul class="list-group" id="chatlistdata">
            </ul>
</div>

<div id="component2">
            
            </div>
    </section>
    `
}

class ChatList extends HTMLElement {
    shadowRoot;

    constructor(Username) {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' })
        this.init();

        var type = this.getAttribute("type");
        if (type === "room") {
            this.initRoom();
        } else if (type === "user") {
            this.initUser();
        } else if (type === "friend") {
            this.initFriend();
        }
    }

    init() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/components/list.css");
        this.shadowRoot.appendChild(linkElem);

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const templateNode = document.createElement('template');
        templateNode.id = listTemplate.id;
        templateNode.innerHTML = listTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#list-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);
    }

    initRoom() {
        const info = document.createElement("additional-info");
        info.setAttribute("maintext", "Online Rooms: ");
        info.setAttribute("amount", 0);
        this.shadowRoot.querySelector("#component1").appendChild(info);
        this.shadowRoot.querySelector("#component2").appendChild(document.createElement("room-buttons"));
    }

    initUser() {
        const users = document.createElement("additional-info");
        users.setAttribute("maintext", "Online Users: ")
        users.setAttribute("amount", 0);
        this.shadowRoot.querySelector("#component1").appendChild(users);

        const info = document.createElement("additional-info");
        info.setAttribute("maintext", "You are: ");
        info.setAttribute("amount", "");
        this.shadowRoot.querySelector("#component2").appendChild(info);
    }

    initFriend() {
        var type = this.getAttribute("myfriends");
        const info = document.createElement("span");

        if (type == 0) info.innerHTML = "Vriendverzoeken";
        else if (type == 1) info.innerHTML = "Mijn vrienden";

        this.shadowRoot.querySelector("#component1").appendChild(info);
    }

    setUser(username) {
        const info = this.shadowRoot.querySelector("#component2").querySelector("additional-info");
        info.amount = username;
    }

    updateButtons(RoomTitle, IsOwner, currentRoomId) {
        //Enable buttons
        if (currentRoomId != "" && currentRoomId != undefined) {
            this.shadowRoot.querySelector('#component2').querySelector("room-buttons").JoinRoom(currentRoomId, IsOwner, RoomTitle);
        }
    }

    DeletedRoomButtons() {
        this.shadowRoot.querySelector('#component2').querySelector("room-buttons").LeaveRoom()
    }

    updateRooms(roomList, currentRoomId) {
        var type = this.getAttribute("type");
        if (type === "room") {
            const roominfo = this.shadowRoot.querySelector('#component1').querySelector('additional-info');

            const self = this;
            self.shadowRoot.querySelector('#chatlistdata').innerHTML = "";
            roominfo.setAttribute("amount", 0);

            roominfo.setAttribute("amount", roomList.length);

            $.each(roomList, function (index) {
                if (roomList[index].id != currentRoomId) {
                    const roomNode = document.createElement('chat-room');
                    roomNode.setAttribute("data-rid", roomList[index].id);
                    roomNode.setAttribute("data-title", roomList[index].title);
                    self.shadowRoot.querySelector('#chatlistdata').append(roomNode);
                }
            });
        }
    }

    //todo secure connection ids
    updateUsers(userList, myUsername) {
        var type = this.getAttribute("type");
        if (type === "user") {
            const userinfo = this.shadowRoot.querySelector('#component1').querySelector('additional-info');

            const self = this;
            self.shadowRoot.querySelector('#chatlistdata').innerHTML = "";
            userinfo.setAttribute("amount", 0);

            userinfo.setAttribute("amount", userList.length-1);

            $.each(userList, function (index) {
                if (userList[index].userName != myUsername) {
                    const userNode = document.createElement('chat-user');
                    userNode.setAttribute("data-id", userList[index].connectionId);
                    userNode.setAttribute("data-username", userList[index].userName);
                    self.shadowRoot.querySelector('#chatlistdata').append(userNode);
                }
            });
        }
    }

    updateFriendList(userList) {
        var type = this.getAttribute("type");
        const self = this;
        if (type === "friend") {
            $.each(userList, function (index) {
                    const userNode = document.createElement('chat-user');
                   // userNode.setAttribute("data-id", userList[index].connectionId);
                    userNode.setAttribute("data-username", userList[index].userName);
                    self.shadowRoot.querySelector('#chatlistdata').append(userNode);
            });
        }
    }
}

export { ChatList };