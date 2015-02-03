var appManage = angular.module('RouteApp', ['ngRoute', 'restangular']);


appManage.config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {

    $locationProvider.hashPrefix('!');
    $routeProvider.when("/organizes", {
            controller: "organizeController",
            templateUrl: "/Manage/Organizes"
        })
        .when("/index", {
            controller: "indexController",
            templateUrl: "/Manage/Index"
        })
        .when("/users", {
            controller: "userController",
            templateUrl: "/Manage/Users"
        })
        .otherwise({ redirectTo: '/' });
}]);