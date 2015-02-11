

appManage.factory('organizeService', ['Restangular', function (Restangular) {
    var restAngular = Restangular.withConfig(function (Configurer) {
        Configurer.setBaseUrl('/Api');
    });
    var _orgnizeService = restAngular.all("Organize");
    return {
        getOrganizes: function () { return _orgnizeService.getList(); },
        newOrganize: function () {
            return {Title:"新增组织结构",Level:"01",Parent:"集团公司"}
        }
    }
}]);

