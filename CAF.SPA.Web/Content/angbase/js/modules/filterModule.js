define(["angular"], function () {

    var module = angular.module("ngFilterHelper", []);
    module.filter("unsafe", function ($sce) {
        return $sce.trustAsHtml;
    });   
    module.filter("getAge", function () {
        return function(date) {
            return new Date().getFullYear() - date.getFullYear();
        };
    });

});