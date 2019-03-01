﻿function Open() {
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

window.addEventListener('load', function () {
    var allimages = document.getElementsByTagName('img');
    for (var i = 0; i < allimages.length; i++) {
        if (allimages[i].getAttribute('data-src')) {
            allimages[i].setAttribute('src', allimages[i].getAttribute('data-src'));
        }
    }
}, false)