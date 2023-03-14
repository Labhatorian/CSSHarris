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
        super(); // always call super() first in the ctor.
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

    // a getter for the maintext property
    get maintext() {
        return this._maintext;
    }

    // a setter for the maintext property
    set maintext(value) {
        this._maintext = value;
        if (!this._setting) { // check if the flag is false
            this._setting = true; // set the flag to true
            this.setAttribute("maintext", value); // update the attribute as well
            this._setting = false; // reset the flag to false
        }

        this.EditText();
    }

    // a getter for the amount property
    get amount() {
        return this._amount;
    }

    // a setter for the amount property
    set amount(value) {
        this._amount = value;
        if (!this._setting) { // check if the flag is false
            this._setting = true; // set the flag to true
            this.setAttribute("amount", value); // update the attribute as well
            this._setting = false; // reset the flag to false
        }

        this.EditText();
    }

    // a callback for when an attribute changes
    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "maintext") {
            this.maintext = newValue; // update the property as well
        } else if (name === "amount") {
            this.amount = newValue; // update the property as well
        }
    }

    // a list of attributes to observe for changes
    static get observedAttributes() {
        return ["maintext", "amount"];
    }
}

export { AdditionalInfo };