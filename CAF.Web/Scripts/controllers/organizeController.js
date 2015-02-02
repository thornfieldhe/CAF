appManage.controller('organizeController',
         function ($scope, organizeService) {
             $scope.test = function () {
                 $scope.getOrganizes = organizeService.getOrganizes();
             }
         });
