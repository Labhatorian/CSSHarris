class AdditionalInfo extends HTMLElement {
    shadowRoot;
    templateId = 'ai-tpl';
    elementId = 'additionalinfo';

    aiTemplate = {
        id: 'ai-tpl',
        template: `
    <div><small id="additionalinfodata"></small></div>
    `
    }

    constructor() {
        super(); 
        this.shadowRoot = this.attachShadow({ mode: 'open' });
        this._amount = 0;
        this._maintext = "";
        this._setting = false; // a flag to prevent infinite recursion
        this.EditText = this.EditText.bind(this);

        const templateNode = document.createElement('template');
        templateNode.id = this.aiTemplate.id;
        templateNode.innerHTML = this.aiTemplate.template;
        const body = document.querySelector('body');
        body.appendChild(templateNode);

        const template = document.querySelector('#ai-tpl');
        const clone = document.importNode(template.content, true);
        this.shadowRoot.appendChild(clone);
    }

    EditText() {
        //Ugly, binding data is horrible in javascript and webcomponents
        this.shadowRoot.innerHTML = '<div>' + this._maintext + '<small id="additionalinfodata">' + this._amount + '</small></div>';
    }

    get maintext() {
        return this._maintext;
    }

    set maintext(value) {
        this._maintext = value;
        if (!this._setting) { 
            this._setting = true; 
            this.setAttribute("maintext", value); 
            this._setting = false;
        }
        this.EditText();
    }

    get amount() {
        return this._amount;
    }

    set amount(value) {
        this._amount = value;
        if (!this._setting) {
            this._setting = true;
            this.setAttribute("amount", value);
            this._setting = false; 
        }
        this.EditText();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "maintext") {
            this.maintext = newValue;
        } else if (name === "amount") {
            this.amount = newValue; 
        }
    }

    static get observedAttributes() {
        return ["maintext", "amount"];
    }
}

export { AdditionalInfo };