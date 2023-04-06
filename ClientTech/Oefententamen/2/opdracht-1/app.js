
//student uitwerking



let tegelOnClick = function (event) {

    console.log(this);

//student uitwerking
    this.classList.add("tegelDraaien");
    let score = document.getElementsByTagName("p")[0]
    let value = parseInt(score.innerHTML);
    value += 1;
    score.innerHTML = value;
//student uitwerking

    console.log('tegel geklikt');
    return 'tegel geklikt';

}

//document ready
$(() => {


//student uitwerking
 let images = document.getElementsByTagName("img")
 images[0].addEventListener(("click"), tegelOnClick)
 images[1].addEventListener(("click"), tegelOnClick)
});

