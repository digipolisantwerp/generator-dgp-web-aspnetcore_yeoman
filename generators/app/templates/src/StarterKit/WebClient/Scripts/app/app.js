(function() {
    'use strict';
    var app = angular.module('theApp', [
        'ngAnimate',
        'AppService',
        'ui.router',
        'tink.tinkApi',
        'tink.navigation'
    ]);

    angular.module('theApp')
        .filter('to_trusted', ['$sce', function($sce) {
            return function(text) {
                return $sce.trustAsHtml(text);
            };
        }]);

    app.config(function($stateProvider, $urlRouterProvider, $httpProvider, AppConfig) {

        $urlRouterProvider.otherwise("/");

        $stateProvider
            .state("app", {
                abstract: true,
                templateUrl: AppConfig.templateUrl + "scripts/app/subview.html"
            })
            .state("app.home", {
                url: "/",
                controller: "HomeController as ctrl",
                controllerAs: 'ctrl',
                templateUrl: AppConfig.templateUrl + "scripts/app/home/home.html",
                data: {}
            })


        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        //$locationProvider.html5Mode(true);

    }).run(['$rootScope', 'AppService', function($rootScope, appService) {
        // Todo : insert your startup logic here
    }]);

})();
