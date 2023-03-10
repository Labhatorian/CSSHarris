
const chatTemplate = {
    id: 'chat-pane-tpl',
    template: `
    <section id="chat">
            <div class="bg-light user-list" id="chatpane">
            <div class="">
                <div class="">
                    <ul id="messagesList"></ul>
                </div>
            </div>
        </div>
            <br />
            <div class="bg-light" id="chatinput">
                <div class=""><input type="text" class="w-100" id="messageInput" /></div>
                <br />
                <input type="button" id="sendButton" value="Send Message" />
            </div>
    </section> 
    `
}

const messageTemplate = {
    id: 'chatmessage-tpl',
    template: `
    <li>test</li>
    `
}

class ChatTemplate {

    attachTemplates() {
        this.attachTemplate(appTemplate);
        this.attachTemplate(cirkelTemplate);
    }

    attachTemplate(tplObject) {
        const templateNode = document.createElement('template');
        templateNode.id = tplObject.id;
        templateNode.innerHTML = tplObject.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);
    }

}

const ChatTemplate = new ChatTemplate();

export { ChatTemplate };
