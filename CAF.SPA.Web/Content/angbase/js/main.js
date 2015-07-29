require.config({
    urlArgs: "v=" + new Date().getTime(),
    waitSeconds: 300,
    baseUrl: "/Content/angbase",
    paths: {       
        app: "js/app",
        angular: "lib/angular.min",
        ngRoute: "lib/angular-route.min",
        ngResource: "lib/angular-resource.min",
        directiveModule: "js/modules/directiveModule",
        filterModule: "js/modules/filterModule",
    },
    shim: {
        app: {
            exports: "app",
            deps: [
                "angular", "ngRoute", "ngResource", "directiveModule", "filterModule"
            ]
        },
        angular: {
            exports: "angular"
        },
        ngRoute: {
            deps: [
                "angular"
            ]
        },
        ngResource: {
            deps: [
                "angular"
            ]
        },
        directiveModule: {
            deps: [
                "angular"
            ]
        },
        filterModule: {
            deps: [
                "angular"
            ]
        }
    }
});

require(["app"], function () {
    angular.element(document).ready(function () {
        angular.bootstrap(document, ["angbaseApp"]);
    });
});