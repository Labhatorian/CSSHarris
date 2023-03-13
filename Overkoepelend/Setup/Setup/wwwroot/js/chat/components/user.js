const userTemplate = {
    id: 'user-tpl',
    template: `
    <li class="list-group-item user">
                    <a href="#">
                        <div class="username"></div>
                    </a>
                </li>
    `
}

class User extends HTMLElement {

    shadowRoot;
    templateId = 'user-tpl';

    constructor() {
        super(); // always call super() first in the ctor.
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
        // Vind het p element met class title
        let div = this.shadowRoot.querySelector(".username");
        // Wijs de titel toe aan het p element
        div.textContent = this._username;
    }

    // a getter for the amount property
    get username() {
        return this._username;
    }

    // a setter for the amount property
    set username(value) {
        this._username = value;
        if (!this._setting) { // check if the flag is false
            this._setting = true; // set the flag to true
            this.setAttribute("data-username", value); // update the attribute as well
            this._setting = false; // reset the flag to false
        }

        this.EditText();
    }

    // a callback for when an attribute changes
    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "data-username") {
            this.username = newValue; // update the property as well
        }
    }

    // a list of attributes to observe for changes
    static get observedAttributes() {
        return ["data-username"];
    }
}

export { User };
