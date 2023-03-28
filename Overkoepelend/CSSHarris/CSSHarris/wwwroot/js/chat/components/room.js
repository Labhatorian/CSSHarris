const roomTemplate = {
    id: 'room-tpl',
    template: `
    <li class="list-group-item room">
                    <a href="#">
                        <div class="title"></div>
                    </a>
                </li>
    `
}

class Room extends HTMLElement {
    shadowRoot;
    templateId = 'room-tpl';

    constructor() {
        super();
        this.shadowRoot = this.attachShadow({ mode: 'open' });

        this._title = '';
        this._setting = false;

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const templateNode = document.createElement('template');
        templateNode.id = roomTemplate.id;
        templateNode.innerHTML = roomTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#room-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);

        this.applyEventlisteners();
    }

    EditText() {
        let div = this.shadowRoot.querySelector(".title");
        div.textContent = this._title;
    }

    get title() {
        return this._title;
    }

    set title(value) {
        this._title = value;
        if (!this._setting) { 
            this._setting = true; 
            this.setAttribute("data-title", value); 
            this._setting = false; 
        }
        this.EditText();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "data-title") {
            this.title = newValue; 
        }
    }

    static get observedAttributes() {
        return ["data-title"];
    }

    applyEventlisteners() {
       this.shadowRoot.querySelector("a").addEventListener('click', () => {
            console.log('Joining room...');

            var event = new CustomEvent("joinroom", {
                composed: true, 
                bubbles: true, 
            });
           event.roomid = this.getAttribute('data-rid');

            this.shadowRoot.dispatchEvent(event);
        });
    }
}

export { Room };