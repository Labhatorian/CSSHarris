import BoodschappenItem from './components/boodschappen-item.js';

class App extends HTMLElement {

    shadowRoot;
    templateId = 'boodschappen-app-tpl';
    elementId = 'boodschappen-app';

    constructor() {
        super();
        this.shadowRoot = this.attachShadow({mode: 'open'})
    }

    connectedCallback() {
        this.applyTemplate();
        this.attachStyling();

        // for(const p of ['melk', 'kaas', 'brood']){
        //     const item = new BoodschappenItem(p);
        //     this.shadowRoot.querySelector('ul').appendChild(item);
        // }

//student uitwerking
        for(const p of ['melk', 'kaas', 'brood']){
            const item = new BoodschappenItem(p);
            this.shadowRoot.querySelector('ul').appendChild(item);
        }
    }

    applyTemplate() {
        let appTemplate = document.getElementById(this.templateId);

        //clone template an voeg toe aan shadowRoot
//student uitwerking

        const clone = document.importNode(appTemplate.content, true);
        this.shadowRoot.appendChild(clone);
    }

    // app.js
    attachStyling() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "stylesheets/app.css");
        this.shadowRoot.appendChild(linkElem);
    }

}

customElements.define('boodschappen-app', App);
