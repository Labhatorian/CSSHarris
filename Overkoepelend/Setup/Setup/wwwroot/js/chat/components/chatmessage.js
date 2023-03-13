class ChatMessage extends HTMLElement {
    shadowRoot;
    constructor(user, message) {
        super(); 
        this.shadowRoot = this.attachShadow({ mode: 'open' });

        let li = document.createElement("li");
        li.textContent = user + ": " + message;

        this.shadowRoot.appendChild(li);
    }
}

export { ChatMessage };