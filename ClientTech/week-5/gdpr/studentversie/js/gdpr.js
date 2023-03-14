//DIT IS IN MODULE PATTERN
const gdpr = (function () {
    const init =  () => {
        showStatus();
        showContent();
        bindEvents();

        if(cookieStatus() !== 'accept') showGDPR();
    }

    const bindEvents = (function () {
        let buttonAccept = document.querySelector('.gdpr-consent__button--accept');
        buttonAccept.addEventListener('click', () => {
            cookieStatus('accept');
            showStatus();
            showContent();
            hideGDPR();
        });

        let buttonReject = document.querySelector('.gdpr-consent__button--reject');
        buttonReject.addEventListener('click', () => {
            cookieStatus('reject');
            showStatus();
            showContent();
            hideGDPR();
        });
    })();


    const showContent = (function () {
        resetContent();
        const status = cookieStatus() == null ? 'not-chosen' : cookieStatus();
        if(status !== 'not-chosen') showGDPRTimeDate();
        const element = document.querySelector(`.content-gdpr-${status}`);
        element.classList.add('show');
    })();

    const resetContent = (function () {
        const classes = [
            '.content-gdpr-accept',
            '.content-gdpr-not-chosen',
            '.content-gdpr-reject'];

        for(const c of classes){
            document.querySelector(c).classList.add('hide');
            document.querySelector(c).classList.remove('show');
        }

        document.getElementById('content-gdpr-timedate__desc').innerHTML = 'De GDPR keuze is genomen op: ';
    })();

    const showStatus = (function () {
        document.getElementById('content-gpdr-consent-status').innerHTML =
            cookieStatus() == null ? 'Niet gekozen' : cookieStatus();
        })();

    const cookieStatus = (function (status) {
        if (status) localStorage.setItem('gdpr-consent-choice', status);

        if(status){
            let datetime = { datum: new Date().toLocaleDateString('nl-NL'), tijd: new Date().toLocaleTimeString('en-GB', { hour: "numeric", 
                minute: "numeric"})};
            localStorage.setItem('gdpr-consent-datetime', JSON.stringify(datetime));
        }

        return localStorage.getItem('gdpr-consent-choice');
    })();

    const showGDPRTimeDate = (function () {
        document.querySelector(`.content-gdpr__timedate`).classList.remove('hide');
        document.querySelector(`.content-gdpr__timedate`).classList.add('show');

        let datetime = JSON.parse(localStorage.getItem('gdpr-consent-datetime'));
        document.getElementById('content-gdpr-timedate__desc').innerHTML += JSON.stringify(datetime);
    })();

    const hideGDPR = (function () {
        document.querySelector(`.gdpr-consent`).classList.add('hide');
        document.querySelector(`.gdpr-consent`).classList.remove('show');
    })();

    const showGDPR = (function () {
        document.querySelector(`.gdpr-consent`).classList.add('show');
    })();

    return {
        init: init
    };
})();