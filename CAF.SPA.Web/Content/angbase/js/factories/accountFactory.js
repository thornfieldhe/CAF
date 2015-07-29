define(["app"], function (app) {

    app.register.factory("AccountFactory", function ($resource) {
        return $resource("/account/register", {}, {
            register: { method: "POST", params: {}, isArray: false },
            login: { method: "POST", params: {}, isArray: false }
        });
    });

});
