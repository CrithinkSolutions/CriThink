﻿(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var logo = document.getElementsByClassName('link');
            logo[0].children[0].src = "../logo.svg";
        });
    });
})();