const lijsten = [
    ['melk', 'brood', 'eieren', 'kaas'],
    ['appels', 'peren', 'bananen', 'kiwi'],
    ['aardappelen', 'wortelen', 'paprika', 'ui'],
    ['koffie', 'thee', 'peren', 'chocolademelk', 'cappuccino'],
    ['kip', 'varkensvlees', 'rundvlees', 'vis'],
    ['sinaasappelsap', 'appelsap', 'perensap', 'kiwisap'],
    ['aardbeien', 'frambozen', 'blauwe bessen', 'bosbessen', 'peren']
];

const SPA = (() => {

    return {
        init(lijsten) {
            //SPA.Boodschappen.init(lijsten);

//student uitwerking
        SPA.Boodschappen.init(lijsten);
        }
    }
})();


//student uitwerking
SPA.Boodschappen = (function() {
    let items;

    function init(lijst) {
        items = lijst.flat(2);
    };

    function bereken() {
        const counts = {};
        var max = 0;

        for (const num of items) {
        counts[num] = counts[num] ? counts[num] + 1 : 1;
        if (counts[num] > max){
            max = counts[num];
        }
        }

        for (let [key, value] of Object.entries(counts)) {
            console.log(key + "  " + value);
        }

        console.log("");

        //largest item
        items.forEach(element => {
            if(counts[element] == max)
            console.log(element + "  " + max);
        });

        console.log("");
        // Create items array
        var sorted = Object.keys(counts).map(function(key) {
            return [key, counts[key]];
        });
  
        // Sort the array based on the second element
        sorted.sort(function(first, second) {
            return second[1] - first[1];
        });

        sorted.forEach(element => {
            console.log(element);
        });
    };

    return {
        init: init,
        bereken: bereken
    }

})();

//tip, Array.flat. Voorbeeld: console.log(lijsten.flat(2));
//SPA.Boodschappen.bereken();
//student uitwerking

console.log(lijsten.flat(2));
SPA.init(lijsten);
SPA.Boodschappen.bereken();