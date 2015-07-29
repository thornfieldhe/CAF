define(["app"], function(app) {

    app.register.filter("dolarToTL", function() {
        return function(dolar) {
            return dolar * 2.2;
        };
    });

});