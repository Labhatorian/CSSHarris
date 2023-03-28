
export default class Boodschappen{

    selector;

    constructor(selector) {
        this.selector = selector;
        this.data = [];
    }

    setLijst(data){
        this.data = [...this.data, ...data];
    }

    addBoodschap(boodschap){
        this.data.push(boodschap);
    }

    render(){
//student uitwerking
        let lijst = document.getElementById('boodschappen-lijst');
        lijst.innerHTML = "";
        this.data.forEach(item => {
            lijst.innerHTML += "<li>" + item + "</li>";
        });
    }

}
