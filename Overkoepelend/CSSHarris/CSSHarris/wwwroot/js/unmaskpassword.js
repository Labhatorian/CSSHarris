function showPassword() {
    var x = document.getElementById("PasswordField");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}