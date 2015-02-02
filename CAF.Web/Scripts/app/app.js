var appManage = angular.module('RouteApp', ['ngRoute', 'restangular']);

appManage.config(function($routeProvider) {
    $routeProvider.when("/organizes", {
        controller: "organizeController",
        templateUrl: "/Manage/Organizes"
    });

    $routeProvider.when("/index", {
        controller: "indexController",
        templateUrl: "/Manage/Index"
    });

    $routeProvider.when("/users", {
        controller: "userController",
        templateUrl: "/Manage/Users"
    });
});