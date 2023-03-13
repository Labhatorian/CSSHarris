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
        super(); // always call super() first in the ctor.
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
        // Vind het p element met class title
        let div = this.shadowRoot.querySelector(".title");
        // Wijs de titel toe aan het p element
        div.textContent = this._title;
    }

    // a getter for the amount property
    get title() {
        return this._title;
    }

    // a setter for the amount property
    set title(value) {
        this._title = value;
        if (!this._setting) { // check if the flag is false
            this._setting = true; // set the flag to true
            this.setAttribute("data-title", value); // update the attribute as well
            this._setting = false; // reset the flag to false
        }

        this.EditText();
    }

    // a callback for when an attribute changes
    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "data-title") {
            this.title = newValue; // update the property as well
        }
    }

    // a list of attributes to observe for changes
    static get observedAttributes() {
        return ["data-title"];
    }

    applyEventlisteners() {
       this.shadowRoot.querySelector("a").addEventListener('click', () => {
            console.log('Joining room...');

            var event = new CustomEvent("joinroom", {
                composed: true, // Laat de gebeurtenis doordringen door schaduw-DOM grenzen
                bubbles: true, // Laat de gebeurtenis opborrelen door DOM boom
            });
           event.roomid = this.getAttribute('data-rid');

            this.shadowRoot.dispatchEvent(event);
        });
    }
}

export { Room };
