const buttonsTemplate = {
    id: 'buttons-tpl',
    template: `
    <div class="card card-body bg-light actions">
            <button id="createbutton" class="btn createroom">Create Room</button>
        </div>

        <div class="card card-body bg-light actions">
            <div id="callstatus" class="status">Idle</div>
            <button id="leavebutton" class="btn btn-danger hangup hide">Leave</button>
            <button id="deletebutton" class="btn btn-danger delete hide">Delete Room</button>
        </div>
    `
}

class RoomButtons extends HTMLElement {

    shadowRoot;
    templateId = 'buttons-tpl';
    elementId = 'buttons';

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.state = {
            username: "",
        };

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "../js/chat/css/buttons.css");
        this.shadowRoot.appendChild(linkElem);

        const templateNode = document.createElement('template');
        templateNode.id = buttonsTemplate.id;
        templateNode.innerHTML = buttonsTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#buttons-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);

        this.applyEventlisteners();
    }

    applyEventlisteners() {
        this.addEventListener('leftclick', this.sendEvent);
    }

    sendEvent() {
        this.shadowRoot.dispatchEvent(new Event('userLeftClick', { composed: true }));
    }
}

export { RoomButtons };