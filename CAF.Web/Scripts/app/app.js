var appManage = angular.module('RouteApp', ['ngMessages','ngRoute', 'restangular']);


appManage.config([
        '$locationProvider', '$routeProvider', function($locationProvider, $routeProvider) {
            //            $locationProvider.html5Mode(false).hashPrefix('!');
        $locationProvider.html5Mode(true);
        $routeProvider.when("/Organizes", {
                controller: "organizeController",
                templateUrl: "/Manage/Organizes",
                reloadOnSearch: false
            })
            .when("/Index", {
                controller: "indexController",
                templateUrl: "/Manage/Index"
            })
            .when("/Users", {
                controller: "userController",
                templateUrl: "/Manage/Users"
            });
    }
    ]);