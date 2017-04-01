var UTILS = (function (module) {
    //logout
    module.logout = function () {
        document.getElementById("logoutForm").submit();
    }

    return module;
}(UTILS || {}));