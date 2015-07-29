angular.module("app", [])
    .controller("regionCtrl", [
        '$scope', '$http', function ($scope, $http) {
            $scope.submit=function() {
                $http.post("/Manage/User/Region", { model: $scope.user}).success(function (e) {
                    if(e.Status!==1) {
                        window.location.href = e.Data;
                    }else {
                        $scope.user.errors = e.Message;
                    }
                })}
    
}]);