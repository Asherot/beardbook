var UTILS = (function (module) {
    //activate tabs
    var hash = window.location.hash;
    hash && $('ul.nav a[href="' + hash + '"]').tab("show");

    $(".nav-tabs a").click(function () {
        $(this).tab("show");
        window.location.hash = this.hash;
    });

    //activate OptionsBar pills
    if (document.title.includes("Profile")) {
        document.getElementById("menu-profile").className = "active";
    }
    else if (document.title.includes("Conversations")) {
        document.getElementById("menu-conversations").className = "active";
    }
    else {
        document.getElementById("menu-home").className = "active";
    }

    return module;
}(UTILS || {}));