const messageTemplate = {
    id: 'chatmessage-tpl',
    template: `
    <li>test</li>
    `
}

class ChatMessage extends HTMLElement {

    shadowRoot;
    templateId = 'chat-pane-tpl';
    elementId = 'chatmessage';

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.state = {
            username: "",
            message: "",
            datetime: ""
        };
        this.applyTemplate();
        this.applyEventlisteners();
    }

    applyTemplate() {
        const templateNode = document.createElement('chatMessage');
        templateNode.id = messageTemplate.id;
        templateNode.innerHTML = messageTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);
    }

    applyEventlisteners() {
        this.addEventListener('rightclick', this.sendEvent);
    }

    sendEvent() {
        this.shadowRoot.dispatchEvent(new Event('messageRightClick', { composed: true }));
    }
}

customElements.define('chat-message', ChatMessage);