import {Model} from "./model.js";

const model = new Model();

class HogerLagerFeature{
    init(){
        console.log('HogerLagerFeature init');

//student uitwerking
        this.eventListeners();
    }

    async getGetal(){
        let gok = document.getElementById("gok").value;
        let result = await model.getGetal(gok);
        console.log(result);
    }


    eventListeners(){
        document.getElementById('btnSubmit')
            .addEventListener('click', this.submitGuess);
    }

    async submitGuess(event){

//student uitwerking
        event.preventDefault();
        let guess = document.getElementById('guess').value;
        const answer = await model.getGetal(guess);
        document.getElementById('answer').innerHTML = answer;
    }
}

export {HogerLagerFeature};
