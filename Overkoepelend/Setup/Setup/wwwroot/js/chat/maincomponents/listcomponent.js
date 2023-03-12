import { RoomButtons } from "../components/buttons.js";
import { AdditionalInfo } from "../components/additionalinfo.js";

customElements.define('room-buttons', RoomButtons);
customElements.define('additional-info', AdditionalInfo);

const listTemplate = {
    id: 'list-tpl',
    template: `
     <section id="chatlist">
            <div id="component1">
         
            </div>
            <div class="bg-light room-list" id="chatlistdata">
            <ul class="list-group">
                <li class="list-group-item item" data-id="" data-title="">
                    <a href="#">
                        <div class="title"></div>
                    </a>
                </li>
            </ul>
</div>

<div id="component2">
            
            </div>
    </section>
    `
}

class ChatList extends HTMLElement {
    shadowRoot;

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' })
        this.init();

        var type = this.getAttribute("type");
        if (type === "room") {
            this.initRoom();
        } else if (type === "user") {
            this.initUser();
        }
    }

    init() {
        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "../js/chat/css/list.css");
        this.shadowRoot.appendChild(linkElem);

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const templateNode = document.createElement('template');
        templateNode.id = listTemplate.id;
        templateNode.innerHTML = listTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#list-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);
    }

    initRoom() {
        this.shadowRoot.querySelector("#component1").appendChild(document.createElement("additional-info"));
        this.shadowRoot.querySelector("#component2").appendChild(document.createElement("room-buttons"));
    }

    initUser() {
        this.shadowRoot.querySelector("#component1").appendChild(document.createElement("additional-info"));
    }
}

export { ChatList };