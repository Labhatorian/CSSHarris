const userTemplate = {
    id: 'user-tpl',
    template: `
    <li class="list-group-item user" data-cid="" data-username="">
                    <a href="#">
                        <div class="username"></div>
                        <div class="helper" data-bind="css: $parent.getUserStatus($data)"></div>
                    </a>
                </li>
    `
}

class User extends HTMLElement {

    shadowRoot;
    templateId = 'chatlist-tpl';
    elementId = 'user';

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.state = {
            username: "",
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
        linkElem.setAttribute("href", "stylesheets/components/user.css");
        this.shadowRoot.appendChild(linkElem);
    }

    applyEventlisteners() {
        this.addEventListener('leftclick', this.sendEvent);
    }

    sendEvent() {
        this.shadowRoot.dispatchEvent(new Event('userLeftClick', { composed: true }));
    }
}

customElements.define('user', User);

export { User };
