define(["app"], function (app) {

    app.register.directive("ngYearDirective", function () {
        return {
            restrict: "E",
            template: "yıl : " + new Date().getFullYear() + " (required ile yüklendi )"
        };
    });

    app.register.directive("ngMinuteDirective", function () {
        return {
            restrict: "A",
            template: "dakika : " + new Date().getMinutes() + " (required ile yüklendi )"
        };
    });
    
});