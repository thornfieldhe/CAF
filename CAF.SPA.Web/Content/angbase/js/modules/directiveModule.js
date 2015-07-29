define(["angular"], function (angular) {

    var module = angular.module("ngDirectiveHelper", []);
    module.directive("yearDirective", function () {
        return {
            restrict: "EA",
            template: "year-directive : " + new Date().getFullYear()
        };
    });
    module.directive("pathDirective", function ($location) {
        return {
            restrict: "EA",
            template: "path-directive : " + $location.$$url
        };
    });

});