const roomTemplate = {
    id: 'room-tpl',
    template: `
    <li class="list-group-item room" data-rid="" data-title="">
                    <a href="#">
                        <div class="title"></div>
                    </a>
                </li>
    `
}

class Room extends HTMLElement {

    shadowRoot;
    templateId = 'chatlist-tpl';
    elementId = 'room';

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.state = {
            id: "",
            name: "",
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
        linkElem.setAttribute("href", "stylesheets/components/room.css");
        this.shadowRoot.appendChild(linkElem);
    }

    applyEventlisteners() {
        this.addEventListener('click', this.sendEvent);
    }

    sendEvent() {
        this.shadowRoot.dispatchEvent(new Event('roomClick', { composed: true }));
    }
}

customElements.define('room', Room);

export { Room };
