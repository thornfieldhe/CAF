define(["app"], function (app) {

    app.register.factory("HomeFactory", function ($resource) {
        return $resource("/home/:action", {}, {
            contact: { method: "POST", params: {action:"contact"}, isArray: false }           
        });
    });

});
