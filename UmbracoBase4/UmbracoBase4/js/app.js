function Open() {
    try {
        el = window.document.getElementById("sidebar");
        el.setAttribute("open", "");
    }
    catch (err) {
        Log(err);
    }
}
function Close() {
    try {
        el = window.document.getElementById("sidebar");
        el.removeAttribute("open");
    }
    catch (err) {
        Log(err);
    }
}
function Log(message) {
        console.log(message);
}