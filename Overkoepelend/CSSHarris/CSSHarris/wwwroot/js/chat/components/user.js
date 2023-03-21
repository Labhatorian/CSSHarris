const userTemplate = {
    id: 'user-tpl',
    template: `
    <li class="list-group-item user">
                    <div class="username"></div>
                </li>
    `
}

class User extends HTMLElement {

    shadowRoot;
    templateId = 'user-tpl';

    constructor() {
        super();
        this.shadowRoot = this.attachShadow({ mode: 'open' });

        this._username = '';
        this._setting = false;

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const templateNode = document.createElement('template');
        templateNode.id = userTemplate.id;
        templateNode.innerHTML = userTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#user-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);

    }

    EditText() {
        let div = this.shadowRoot.querySelector(".username");
        div.textContent = this._username;
    }

    get username() {
        return this._username;
    }

    set username(value) {
        this._username = value;
        if (!this._setting) { 
            this._setting = true;
            this.setAttribute("data-username", value); 
            this._setting = false; 
        }
        this.EditText();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "data-username") {
            this.username = newValue;
        }
    }

    static get observedAttributes() {
        return ["data-username"];
    }
    }

export { User };