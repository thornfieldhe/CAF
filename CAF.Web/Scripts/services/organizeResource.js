var appManage = angular.module('Manage');

appManage.factory('organizeResource',
    ['$scope',
  function ($resource) {
      return $resource( "Organize/:id", {id:'@'}, {
          'get': { method: 'GET', url: 'Organize/:id' },
          'create': { method: 'POST', url: 'Organize/Create' },
          'update': { method: 'PUT', url: 'Organize/Update' },
          'delete': { method: 'DELETE', url: 'Organize/Delete/:id' },
      });
  }]);