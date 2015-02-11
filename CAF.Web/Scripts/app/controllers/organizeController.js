appManage.controller('organizeController',
         function ($scope, organizeService) {
             $scope.newOrganize = function () {
                 console.log("xxx");
                 $scope.Organize = organizeService.newOrganize();
                 $scope.$watch("Organize.Level", function(newAccount) {
                     console.log(123);
                 });
             }
         });
