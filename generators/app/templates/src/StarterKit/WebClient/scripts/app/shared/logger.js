(function() {

    'use strict';

    var componentName = "Logger";
    var theComponent = function($log) {

        function _success(message) {
            $log.log(message);
        }

        function _debug(message) {
            $log.debug(message);
        }

        function _info(message) {
            $log.info(message);
        }

        function _warn(message) {
            $log.warn(message);
        }

        function _error(message) {
            $log.error(message);
        }

        function _creation(name) {
            $log.debug(name + " : gecreëerd.");
        }

        function _initialization(name) {
            $log.debug(name + " : geïnitialiseerd.");
        }

        /* +++++ public interface +++++ */

        return {
            success: _success,
            debug: _debug,
            info: _info,
            warn: _warn,
            error: _error,
            creation: _creation,
            init: _initialization
        };

    };

    theComponent.$inject = ['$log'];

    angular.module('AppService').factory(componentName, theComponent);

})();