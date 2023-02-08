window.onload = function () {
    DisplayViewcounter();
};

function DisplayViewcounter() {
    let counter = getCookie("PageViews");
    document.getElementsByClassName("content-gdpr-accept")[0].innerHTML = "Deze view is " + counter.toString() + " keer bekeken";
}

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