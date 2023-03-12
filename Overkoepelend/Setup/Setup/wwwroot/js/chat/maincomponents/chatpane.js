
const chatpaneTemplate = {
    id: 'chat-pane-tpl',
    template: `
    <section id="chat">
            <div class="bg-light user-list" id="chatpane">
            <div class="">
                <div class="">
                    <ul id="messagesList">
                        <chat-message></chat-message>
                    </ul>
                </div>
            </div>
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
        linkElem.setAttribute("href", "../js/chat/css/chatpane.css");
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
    }
}

export { ChatPane };