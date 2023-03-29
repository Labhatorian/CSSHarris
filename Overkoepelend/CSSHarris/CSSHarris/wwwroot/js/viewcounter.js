window.onload = function () {
    DisplayViewcounter();
};

//Replace text with viewcounter
function DisplayViewcounter() {
    let counter = getCookie("PageViews");
    if (counter === null) counter = 1;
    document.getElementsByClassName("content-gdpr-accept")[0].innerHTML = "Deze view is " + counter.toString() + " keer bekeken";
}

//Get cookie from browser (why isnt this native in JS?)
function getCookie(cookieName) {
    var name = cookieName + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if ((c.indexOf(name)) == 0) {
            return c.substr(name.length);
        }
    }
    return null;
}