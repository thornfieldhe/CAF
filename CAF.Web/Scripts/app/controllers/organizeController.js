appManage.controller('organizeController',
         function ($scope, organizeService) {
             $scope.test = function () {
                 console.log('ddd');
                 $scope.getOrganizes = organizeService.getOrganizes();
             }
         });
