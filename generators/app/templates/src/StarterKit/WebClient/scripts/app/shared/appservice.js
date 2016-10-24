// Contains common functionality like logging, ...

(function() {

    'use strict';

    var componentName = "AppService";
    var theComponent = function(logger) {

        /* +++++ public interface +++++ */

        logger.creation(componentName);

        return {
            logger: logger
        };

    }

    theComponent.$inject = ['Logger'];

    var appServiceModule = angular.module('AppService', []);

    appServiceModule.factory(componentName, theComponent);

})();
