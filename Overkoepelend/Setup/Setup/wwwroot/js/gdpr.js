class GDPR {

    constructor() {
        this.showContent();
        this.bindEvents();

        if(this.cookieStatus() !== 'accept') this.showGDPR();
    }

    //Button events
    bindEvents() {
        let buttonAccept = document.querySelector('.gdpr-consent__button--accept');
        buttonAccept.addEventListener('click', () => {
            this.cookieStatus('accept');
            this.showContent();
            this.hideGDPR();
        });


        let buttonReject = document.querySelector('.gdpr-consent__button--reject');
        buttonReject.addEventListener('click', () => {
            this.cookieStatus('reject');
            this.showContent();
            this.hideGDPR();
        });
    }

    //Content
    showContent() {
        this.resetContent();
        const status = this.cookieStatus() == null ? 'not-chosen' : this.cookieStatus();
        const element = document.querySelector(`.content-gdpr-${status}`);
        if (element != null) element.classList.add('show');
    }

    resetContent(){
        const classes = [
            '.content-gdpr-accept',
            '.content-gdpr-not-chosen',
            '.content-gdpr-reject'];

        for (const c of classes) {
            if (document.querySelector(c) != null) {
                document.querySelector(c).classList.add('hide');
                document.querySelector(c).classList.remove('show');
            }
        }
    }

    //Set and get cookie status
    cookieStatus(status) {
        if (status) localStorage.setItem('gdpr-consent-choice', status);
        if (status == 'accept') document.cookie = 'gdpr=accept'
        return localStorage.getItem('gdpr-consent-choice');
    }

    //GDPR box
    hideGDPR(){
        document.querySelector(`.gdpr-consent`).classList.add('hide');
        document.querySelector(`.gdpr-consent`).classList.remove('show');
    }

    showGDPR(){
        document.querySelector(`.gdpr-consent`).classList.add('show');
    }
}

const gdpr = new GDPR();