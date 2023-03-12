const aiTemplate = {
    id: 'ai-tpl',
    template: `
    <div class="list-group-item">[Additional info]: <small id="additionalinfodata">0</small></div>
    `
}

class AdditionalInfo extends HTMLElement {

    shadowRoot;
    templateId = 'ai-tpl';
    elementId = 'additionalinfo';

    constructor() {
        super(); // always call super() first in the ctor.
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this.state = {
            data: 0
        };

        const linkElemBootstrap = document.createElement("link");
        linkElemBootstrap.setAttribute("rel", "stylesheet");
        linkElemBootstrap.setAttribute("href", "/lib/bootstrap/dist/css/bootstrap.min.css");
        this.shadowRoot.appendChild(linkElemBootstrap);

        const templateNode = document.createElement('template');
        templateNode.id = aiTemplate.id;
        templateNode.innerHTML = aiTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#ai-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);
    }
}

export { AdditionalInfo };