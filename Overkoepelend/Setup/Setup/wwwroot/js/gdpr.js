class GDPR {

    constructor() {
        this.showStatus();
        this.showContent();
        this.bindEvents();

        if(this.cookieStatus() !== 'accept') this.showGDPR();
    }

    bindEvents() {
        let buttonAccept = document.querySelector('.gdpr-consent__button--accept');
        buttonAccept.addEventListener('click', () => {
            this.cookieStatus('accept');
            this.showStatus();
            this.showContent();
            this.hideGDPR();
        });


        let buttonReject = document.querySelector('.gdpr-consent__button--reject');
        buttonReject.addEventListener('click', () => {
            this.cookieStatus('reject');
            this.showStatus();
            this.showContent();
            this.hideGDPR();
        });
    }


    showContent() {
        this.resetContent();
        //const status = this.cookieStatus() == null ? 'not-chosen' : this.cookieStatus();
        //if(status !== 'not-chosen') this.showGDPRTimeDate();
        //const element = document.querySelector(`.content-gdpr-${status}`);
        //element.classList.add('show');
    }

    resetContent(){
        //const classes = [
        //    '.content-gdpr-accept',
        //    '.content-gdpr-not-chosen',
        //    '.content-gdpr-reject'];

        //for(const c of classes){
        //    document.querySelector(c).classList.add('hide');
        //    document.querySelector(c).classList.remove('show');
        //}

        //document.getElementById('content-gdpr-timedate__desc').innerHTML = 'De GDPR keuze is genomen op: ';
    }

    showStatus() {
        //document.getElementById('content-gpdr-consent-status').innerHTML =
        //    this.cookieStatus() == null ? 'Niet gekozen' : this.cookieStatus();
    }

    cookieStatus(status) {
        if (status) localStorage.setItem('gdpr-consent-choice', status);

        if(status){
            let datetime = { datum: new Date().toLocaleDateString('nl-NL'), tijd: new Date().toLocaleTimeString('en-GB', { hour: "numeric", 
                minute: "numeric"})};
            localStorage.setItem('gdpr-consent-datetime', JSON.stringify(datetime));
        }

        return localStorage.getItem('gdpr-consent-choice');
    }

    showGDPRTimeDate(){
        //document.querySelector(`.content-gdpr__timedate`).classList.remove('hide');
        //document.querySelector(`.content-gdpr__timedate`).classList.add('show');

        //let datetime = JSON.parse(localStorage.getItem('gdpr-consent-datetime'));
        //document.getElementById('content-gdpr-timedate__desc').innerHTML += JSON.stringify(datetime);
    }

    hideGDPR(){
        document.querySelector(`.gdpr-consent`).classList.add('hide');
        document.querySelector(`.gdpr-consent`).classList.remove('show');
    }

    showGDPR(){
        document.querySelector(`.gdpr-consent`).classList.add('show');
    }
}

const gdpr = new GDPR();

