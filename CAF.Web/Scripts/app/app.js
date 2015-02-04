var appManage = angular.module('RouteApp', ['ngRoute', 'restangular']);


appManage.config([
        '$locationProvider', '$routeProvider', function($locationProvider, $routeProvider) {
            $locationProvider.html5Mode(false).hashPrefix('!');
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
                .otherwise({ redorectTo: "/" });
        }
    ]);