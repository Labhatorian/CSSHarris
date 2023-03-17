import { ChatMessage } from "../components/chatmessage.js";

customElements.define('chat-message', ChatMessage);

const chatpaneTemplate = {
    id: 'chat-pane-tpl',
    template: `
    <section id="chat">
            <div class="bg-light user-list" id="chatpane">
                    <ul id="messagesList">  
                    </ul>
                </div>
            <br />
            <div class="bg-light" id="chatinput">
                <div class=""><input type="text" class="w-100" id="messageInput" /></div>
                <br />
                <input type="button" id="sendButton" value="Send Message" />
            </div>
    </section> 
    `
}

class ChatPane extends HTMLElement {
    shadowRoot;

    constructor() {
        super(); // always call super() first in the ctor.
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
        this.shadowRoot.querySelector("#sendButton").addEventListener('click', (event) => {
            event.preventDefault();
            var message = this.shadowRoot.getElementById("messageInput").value;

            var messageevent = new CustomEvent("sendmessage", {
                composed: true, // Laat de gebeurtenis doordringen door schaduw-DOM grenzen
                bubbles: true, // Laat de gebeurtenis opborrelen door DOM boom
            });
            messageevent.message = message;

            this.shadowRoot.dispatchEvent(messageevent);
        });
    }

    NewMessage(user, message) {
        let msg = new ChatMessage(user, message);
        this.shadowRoot.querySelector('#messagesList').append(msg);

        var pane = this.shadowRoot.querySelector('#chatpane')
        pane.scrollTop = pane.scrollHeight - pane.clientHeight;
    }

    ShowMessages(Messages) {
        const self = this;
        $.each(Messages, function (index) {
            self.NewMessage(Messages[index].chatUser.userName, Messages[index].content)
        });

        var pane = this.shadowRoot.querySelector('#chatpane')
        pane.scrollTop = pane.scrollHeight - pane.clientHeight;
    }

    DeleteMessages() {
        this.shadowRoot.querySelector('#messagesList').innerHTML = "";
    }
}

export { ChatPane };