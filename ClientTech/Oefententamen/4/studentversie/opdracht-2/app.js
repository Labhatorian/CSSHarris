import Boodschappen from './boodschappen.js';

class App {

    boodschappen;

    start() {
        // this.boodschappen = new Boodschappen('boodschappen');
        // this.addEventListeners();
//student uitwerking
        this.boodschappen = new Boodschappen('boodschappen');
        this.addEventListeners();
    }

    haalBoodschappenOp() {
        //fetch...
//student uitwerking
        fetch('http://127.0.0.1:3000/boodschappen')           //api for the get request
        .then(response => response.json())
        .then(data => {
            this.boodschappen.setLijst(data);
            this.boodschappen.render();
        });
    }

    voegBoodschapToe() {
        let boodschap = document.getElementById('boodschap-invoer').value;
//student uitwerking
    this.boodschappen.addBoodschap(boodschap);
    document.getElementById('boodschap-invoer').value = "";
    this.boodschappen.render();
    }

    addEventListeners() {
        let self = this;
       document.getElementById(`haal-boodschappen-knop`)
           .addEventListener(`click`, () => self.haalBoodschappenOp());
//student uitwerking
document.getElementById(`voeg-boodschap-toe-knop`)
           .addEventListener(`click`, () => {
            self.voegBoodschapToe();
        });
    }
}

const app = new App();
app.start();
