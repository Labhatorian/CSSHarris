const buttonsTemplate = {
    id: 'buttons-tpl',
    template: `
        <div class="card card-body bg-light actions">
            <button id="createbutton" class="btn btn-primary createroom">Create Room</button>
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
        super();
        this.shadowRoot = this.attachShadow({ mode: 'open' });

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/components/buttons.css");
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

    //Change button states
    JoinRoom(RoomTitle, IsOwner) {
        if ($(this.shadowRoot).find('body').attr("data-mode") !== "idle") {
            $(this.shadowRoot).find('body').attr('data-mode', 'inroom');
            $(this.shadowRoot).find("#callstatus").text('In Room: '+ RoomTitle);
            $(this.shadowRoot).find("#leavebutton").removeClass('hide');
            $(this.shadowRoot).find("#createbutton").addClass('hide');
            if (IsOwner) $(this.shadowRoot).find("#deletebutton").removeClass('hide');
        }
    }

    LeaveRoom() {
        if ($(this.shadowRoot).find('body').attr("data-mode") !== "idle") {
            $(this.shadowRoot).find('body').attr('data-mode', 'inroom');
            $(this.shadowRoot).find("#callstatus").text('Idle');
            $(this.shadowRoot).find("#leavebutton").addClass('hide');
            $(this.shadowRoot).find("#createbutton").removeClass('hide');
            $(this.shadowRoot).find("#deletebutton").addClass('hide');
        }
    }

    applyEventlisteners() {
        this.shadowRoot.querySelector("#createbutton").addEventListener('click', () => {
            var title = prompt("Hoe moet de kamer heten?");
            console.log('Creating room...');

            var event = new CustomEvent("createroom", {
                composed: true,
                bubbles: true,
            });
            event.newroom = title;

            this.shadowRoot.dispatchEvent(event);
        });

        this.shadowRoot.querySelector("#leavebutton").addEventListener('click', () => {
            console.log('leaving....');
            this.LeaveRoom();

            var event = new CustomEvent("leaveroom", {
                composed: true,
                bubbles: true,
            });

            this.shadowRoot.dispatchEvent(event);
        });

        this.shadowRoot.querySelector("#deletebutton").addEventListener('click', () => {
            console.log('deleting....');
            this.LeaveRoom();

            var event = new CustomEvent("deleteroom", {
                composed: true,
                bubbles: true,
            });

            this.shadowRoot.dispatchEvent(event);
        });
    }
}

export { RoomButtons };