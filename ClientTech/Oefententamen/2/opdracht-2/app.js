class Webshop {

//student uitwerking
    constructor(url){
        this.url = url;
        this.data = null;
    };
    haalBeschikbaarheidOp = function() {
        const self = this;
        return fetch(this.url)
        .then(function(response) {
            self.data =  response.json();
        })
        .catch(function(error) {
            console.log(error)
        });
    };

    productAanwezigheidInWinkels = function(productNaam) {
        let winkels = []
        let dad = this.winkeldata;

        this.winkeldata.forEach(value => {
            console.log(productNaam);

                for(let product of value.producten){
                    if(product.naam === productNaam){
                        if(product.aantal > 0){
                            winkels.push(JSON.stringify({winkel: value.winkel, product: product.aantal}));
                        }
                    }
                }
        });
        
        console.log(winkels);
        return winkels;
    };

    beschikbareProducten = function() {
        let table = document.getElementById('producten-tabel').getElementsByTagName('tbody')[0];

       this.data.then(value => {
        console.log(value);
        this.winkeldata = value;

        //Get all producten
        let producten = new Set();
        for (let t of value) {
            for(let product of t.producten){
                producten.add(product.naam);
            }
        }

        for(let product of producten){
            var newRow = table.insertRow();

            var nameCell = newRow.insertCell();
            nameCell.appendChild(document.createTextNode(product));

            var beschikbaarheidCell = newRow.insertCell();
            var beschikbaarheid = this.productAanwezigheidInWinkels(product);

            var beschikbaarheidp = document.createElement("p");
            
            var last = JSON.parse(beschikbaarheid.pop());
            beschikbaarheid.forEach(item => {
                const prod = JSON.parse(item);
                beschikbaarheidp.appendChild(document.createTextNode(prod.winkel + "(" + prod.product + "), "));
            })
            beschikbaarheidp.appendChild(document.createTextNode(last.winkel + "(" + last.product + ")"));
            
            beschikbaarheidCell.appendChild(beschikbaarheidp);   
        }
        });       
    };
}
// Met onderstaande code kun je de tabel vullen, na implementatie methode webshop.beschikbareProducten.

let webshop = new Webshop('producten.json');

webshop.haalBeschikbaarheidOp().then(() => {
    webshop.beschikbareProducten();
})