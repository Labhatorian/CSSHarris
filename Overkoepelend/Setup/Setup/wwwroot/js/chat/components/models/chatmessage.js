class ChatMessage extends HTMLElement {

    shadowRoot;
    templateId = 'chatmessage-tpl';
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
        this.attachStyling();
        this.applyEventlisteners();
    }

    applyTemplate() {
        let template = document.getElementById(this.templateId);
        let clone = template.content.cloneNode(true);
        this.shadowRoot.appendChild(clone);
    }

    attachStyling() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "stylesheets/components/chatmessage.css");
        this.shadowRoot.appendChild(linkElem);
    }

    applyEventlisteners() {
        this.addEventListener('rightclick', this.sendEvent);
    }

    sendEvent() {
        this.shadowRoot.dispatchEvent(new Event('messageRightClick', { composed: true }));
    }
}

customElements.define('chatmessage', ChatMessage);

export { ChatMessage };
