// document ready, de spa applicatie gebruiken om de tekorten op te halen
$(() => {

// na opdracht 3b
// let tekorten = WinkelApp.Statistieken.tekorten(producten);
// console.log(tekorten);

//na opdracht 3c
// let tekorten = WinkelApp.Statistieken.tekorten(producten, (w) => {console.log(w.winkel)});
});



//student uitwerking

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
