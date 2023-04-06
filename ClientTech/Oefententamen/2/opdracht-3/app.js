// document ready, de spa applicatie gebruiken om de tekorten op te halen
$(() => {

// na opdracht 3b
// let tekorten = WinkelApp.Statistieken.tekorten(producten);
// console.log(tekorten);

//na opdracht 3c
// let tekorten = WinkelApp.Statistieken.tekorten(producten, (w) => {console.log(w.winkel)});
// console.log(tekorten);

WinkelApp.runeverysecond();


});

//student uitwerking
const WinkelApp = (() => {
    function runeverysecond(){
        let tekorten = WinkelApp.Statistieken.tekorten(producten, (w) => {console.log(w.winkel)});
        
        tekorten.forEach(tekort => {
            tekort = JSON.parse(tekort);
            console.log("winkel: " + tekort.winkel + ", tekorten: " + tekort.producten + ".");
        })

        setTimeout(arguments.callee, 1000);
    }
    return {
        runeverysecond: runeverysecond
    };
})();

WinkelApp.Statistieken = (() => {

    function tekorten(winkeldata, callback){
        let tekortenWinkels = [];
        winkeldata.forEach(wink => {
            let tekortenlijst = [];

            wink.producten.forEach(product => {
                callback(wink);
                if(product.aantal === 0){
                    tekortenlijst.push(product.naam);
                }
            });    

            if(tekortenlijst.length != 0){

                var last = tekortenlijst.pop();
                let productenstring = "";
                tekortenlijst.forEach(product =>{productenstring += product + ", " });
                productenstring += last;

                tekortenWinkels.push(JSON.stringify({winkel: wink.winkel, producten: productenstring}))
            }
        });

        return tekortenWinkels;
    };

    return {
        tekorten: tekorten
    }
})();
//student uitwerking

let producten = [
    {
        "winkel": "Zwolle",
        "producten": [
            {
                "naam": "grasmaaier",
                "aantal":  100
            },
            {
                "naam": "boormachine",
                "aantal": 1
            }
        ]
    },
    {
        "winkel": "Zutphen",
        "producten": [
            {
                "naam": "grasmaaier",
                "aantal":  5
            },
            {
                "naam": "boormachine",
                "aantal": 2
            }
        ]
    },
    {
        "winkel": "Apeldoorn",
        "producten": [
            {
                "naam": "grasmaaier",
                "aantal":  0
            },
            {
                "naam": "boormachine",
                "aantal": 37
            }
        ]
    }
];
