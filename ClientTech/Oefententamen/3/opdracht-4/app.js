const SPA = (() => {
    const _print = (uitslag) => {
        console.log(uitslag);

        //onderstaande code mag je aanpassen om aan het format in de opdracht te voldoen
        // for (let l of uitslag) {
        //     console.log(`${l.voornaam}, ${l.geslaagd ? "geslaagd" : "niet geslaagd"}`);
        //     if (!l.geslaagd) {
        //         console.log(`niet behaalde toetsen:`);
        //         for (let r of Object.entries(l.resultaten)) {
        //             if (r[1] < 5.5) {
        //                 console.log(`${r[0]} \t ${r[1]}`);
        //             }
        //         }
        //     }
        //     console.log(``)
        // }

//student uitwerking

    uitslag.forEach(l => {
        console.log(`${l.voornaam}, ${l.geslaagd ? "geslaagd" : "niet geslaagd"}`);
            if (!l.geslaagd) {
                console.log(`niet behaalde toetsen:`);
                for (let r of Object.entries(l.resultaten)) {
                    if (r[1] < 5.5) {
                        console.log(`${r[0]} \t ${r[1]}`);
                    }
                }
            }
            console.log(``)
    });

    console.log(``)
    uitslag.sort((a, b) => {
        return a.voornaam.localeCompare(b.voornaam);
    });
    console.log(``)

    uitslag.forEach(l => {
        console.log(`${l.voornaam}, ${l.geslaagd ? "geslaagd" : "niet geslaagd"}`);
            if (!l.geslaagd) {
                console.log(`niet behaalde toetsen:`);
                for (let r of Object.entries(l.resultaten)) {
                    if (r[1] < 5.5) {
                        console.log(`${r[0]} \t ${r[1]}`);
                    }
                }
            }
            console.log(``)
    });

    }
    return {
        print: _print
    };
})();

// Hint sorteren:
//
//strings
// result.sort((a, b) => {
//   return a.voornaam.localeCompare(b.voornaam);
// });
//
// integers
// result.sort((a, b) => {
//     return b.voornaam - a.voornaam;
// });

// Hint berekening:
//  onderstaande code mag je gebruiken, je mag ook zelf een andere oplossing kiezen
//
// //vakken
// let vakken = new Set();
// for (let t of toetsen) {
//     vakken.add(t.vak);
// }
//
// //leerlingen - vakken resultaten array
// for (let l of leerlingen) {
//     l.resultaten = {};
//     for (let v of vakken) {
//         l.resultaten[v] = [];
//     }
//
//     result[l.leerlingNummer] = l;
// }

//student uitwerking
SPA.SlagingsBerekening = (function() {
    function bereken(leerlingen, toetsen) {
        let vakken = new Set();
        const result = []
        for (let t of toetsen) {
         vakken.add(t.vak);
        }
        //leerlingen - vakken resultaten array\
        let leerlingcount = 0;
        for (let l of leerlingen) {
            l.resultaten = {};
            let gemiddeld = 0;
            let countvakken = 0;
            toetsen.forEach(element => {
                if(element.leerling == l.leerlingNummer){
                    gemiddeld = element.cijfer * element.weging;
                    countvakken += 1;
                    if(element.cijfer <= 5.5){
                        l.resultaten[element.vak] = element.cijfer;
                    }
                }
            });
            gemiddeld = gemiddeld / countvakken;

            if(gemiddeld >= 5.5){
                l.geslaagd = true;
            } else {
                l.geslaagd = false;
            }

    result[leerlingcount] = l;
    leerlingcount += 1;
        }
        return result;
    }

    return {
        bereken: bereken
    }

})();

//Leerlingen en toetsen
const leerlingen = [
    {
        leerlingNummer: 's1234567',
        voornaam: 'Marieke'
    },
    {
        leerlingNummer: 's7654321',
        voornaam: 'Evert'
    },
    {
        leerlingNummer: 's1111111',
        voornaam: 'Maurits'
    }
];

const toetsen = [
    {
        leerling: 's1234567',
        vak: 'aardrijkskunde',
        cijfer: 7,
        weging: 2
    },
    {
        leerling: 's1234567',
        vak: 'aardrijkskunde',
        cijfer: 3,
        weging: 1
    },
    {
        leerling: 's1234567',
        vak: 'engels',
        cijfer: 5.4,
        weging: 3
    },
    {
        leerling: 's1234567',
        vak: 'engels',
        cijfer: 5.2,
        weging: 1
    },
    {
        leerling: 's7654321',
        vak: 'aardrijkskunde',
        cijfer: 8,
        weging: 2
    },
    {
        leerling: 's7654321',
        vak: 'aardrijkskunde',
        cijfer: 9,
        weging: 1
    },
    {
        leerling: 's7654321',
        vak: 'engels',
        cijfer: 6.1,
        weging: 3
    },
    {
        leerling: 's7654321',
        vak: 'engels',
        cijfer: 5.2,
        weging: 1
    },
    {
        leerling: 's1111111',
        vak: 'aardrijkskunde',
        cijfer: 10,
        weging: 2
    },
    {
        leerling: 's1111111',
        vak: 'aardrijkskunde',
        cijfer: 8.5,
        weging: 1
    },
    {
        leerling: 's1111111',
        vak: 'engels',
        cijfer: 5.2,
        weging: 3
    },
    {
        leerling: 's1111111',
        vak: 'engels',
        cijfer: 5.8,
        weging: 1
    }
];

$(() => {
    let overzicht = SPA.SlagingsBerekening.bereken(leerlingen, toetsen);
    SPA.print(overzicht);
})
