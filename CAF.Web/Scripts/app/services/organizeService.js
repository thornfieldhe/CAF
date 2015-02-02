

appManage.factory('organizeService', ['Restangular', function (Restangular) {
    var restAngular = Restangular.withConfig(function (Configurer) {
        Configurer.setBaseUrl('/Api');
    });
    var _orgnizeService = restAngular.all("Organize");
    return {
        getOrganizes: function () { return _orgnizeService.getList(); }
    }
}]);

