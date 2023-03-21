import { ChatMessage } from "../components/chatmessage.js";

customElements.define('chat-message', ChatMessage);

const chatpaneTemplate = {
    id: 'chat-pane-tpl',
    template: `
    <section id="chat">
            <div class="bg-light user-list" id="chatpane">
                    <ul id="messagesList" class="no-gutter">  
                    </ul>
                </div>
            <br />
            <div class="bg-light" id="chatinput">
                <div class=""><input type="text" class="w-100" id="messageInput" /></div>
                <br />
                <input type="button" id="sendButton" value="Send Message" />
            </div>

<ul id="contextMenu" class="dropdown-menu" role="menu" style="display:none" >
    <li><a tabindex="-1" href="#">Delete Message</a></li>
</ul>

    </section> 
    `
}

class ChatPane extends HTMLElement {
    shadowRoot;

    constructor() {
        super();
        this.shadowRoot = this.attachShadow({ mode: 'open' })
        this.init();
    }

    init() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/components/chatpane.css");
        this.shadowRoot.appendChild(linkElem);

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const templateNode = document.createElement('template');
        templateNode.id = chatpaneTemplate.id;
        templateNode.innerHTML = chatpaneTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#chat-pane-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);

        this.applyEventlisteners();
    }

    applyEventlisteners() {
        const self = this;

        this.shadowRoot.querySelector("#sendButton").addEventListener('click', (event) => {
            event.preventDefault();
            var message = this.shadowRoot.getElementById("messageInput").value;

            var messageevent = new CustomEvent("sendmessage", {
                composed: true,
                bubbles: true,
            });
            messageevent.message = message;

            this.shadowRoot.dispatchEvent(messageevent);
        });

        if (this.getAttribute("moderator")) {
            var element;
            this.shadowRoot.addEventListener("contextmenu", function (event) {
                if (event.target.tagName === "CHAT-MESSAGE") {
                    element = event.target;

                    event.preventDefault();
                    var contextMenu = self.shadowRoot.getElementById("contextMenu");
                    contextMenu.style.display = "block";
                    contextMenu.style.left = event.pageX + "px";
                    contextMenu.style.top = event.pageY + "px";
                }
            });

            this.shadowRoot.querySelector("#contextMenu li a").addEventListener("click", function (event) {
                var messageid = element.getAttribute("data-id");

                var deleteEvent = new CustomEvent("deleteMessage", {
                    composed: true,
                    bubbles: true,
                });
                deleteEvent.messageid = messageid;

                self.shadowRoot.dispatchEvent(deleteEvent);

                element = null;
            });

            this.shadowRoot.addEventListener("click", function (event) {
                var contextMenu = self.shadowRoot.getElementById("contextMenu");
                contextMenu.style.display = "none";
                element = null;
            });
        }
    }

    NewMessage(id, user, message) {
        let msg = new ChatMessage(user, message);
        this.shadowRoot.querySelector('#messagesList').append(msg);
        msg.setAttribute("data-id", id)
        var pane = this.shadowRoot.querySelector('#chatpane')
        pane.scrollTop = pane.scrollHeight - pane.clientHeight;
    }

    ShowMessages(Messages) {
        this.DeleteMessages();
        const self = this;
        $.each(Messages, function (index) {
            self.NewMessage(Messages[index].id, Messages[index].username, Messages[index].content)
        });

        var pane = this.shadowRoot.querySelector('#chatpane')
        pane.scrollTop = pane.scrollHeight - pane.clientHeight;
    }

    DeleteMessages() {
        this.shadowRoot.querySelector('#messagesList').innerHTML = "";
    }
}

export { ChatPane };