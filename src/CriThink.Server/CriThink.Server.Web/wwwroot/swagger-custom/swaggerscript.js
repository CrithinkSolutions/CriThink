(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var logo = document.getElementsByClassName('link'); 
            logo[0].href = "/";
            logo[0].target = "_blank";
            logo[0].children[0].alt = "Crithink API";
            logo[0].children[0].src = "../logo.svg";
            var name = document.getElementsByTagName('title') 
            name[0].innerHTML = "Crithink API UI"
            var link = document.querySelector("link[rel*='icon']")
            link.href = '../favicon.ico';
        });
    });
})();