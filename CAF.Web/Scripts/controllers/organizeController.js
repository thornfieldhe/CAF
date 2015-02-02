var appManage = angular.module('Manage');

appManage.controller('organizeController',
    ['$scope','organizeResource',
        function ($scope, $routeParams, organizeController) {
            $scope.organize = organizeController.list({ key: '1'});
        }]);
