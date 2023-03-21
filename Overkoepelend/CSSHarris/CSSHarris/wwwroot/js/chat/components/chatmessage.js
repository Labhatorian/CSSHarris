class ChatMessage extends HTMLElement {
    shadowRoot;
    constructor(user, message) {
        super(); 
        this.shadowRoot = this.attachShadow({ mode: 'open' });

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const linkElem = document.createElement("link");
        linkElem.setAttribute("rel", "stylesheet");
        linkElem.setAttribute("href", "/css/components/message.css");
        this.shadowRoot.appendChild(linkElem);

        let li = document.createElement("li");
        li.classList.add("list-group-item", "item");
        li.textContent = user + ": " + message;

        this.shadowRoot.appendChild(li);
    }
}

export { ChatMessage };