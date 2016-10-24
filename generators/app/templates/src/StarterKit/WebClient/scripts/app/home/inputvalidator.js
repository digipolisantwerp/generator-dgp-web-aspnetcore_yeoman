// Example of a component (service) that can be injected in a controller.

(function() {

    'use strict';

    var componentName = "InputValidator";
    var theComponent = function(appService) {

        function _validate(input) {

            var result = {
                isValid: true,
                messages: []
            };

            if (input === undefined || input === null) {
                result.isValid = false;
                result.messages.push("No input.");
                return result;
            }

            if (input.field1 === undefined) {
                result.isValid = false;
                result.messages.push("field1 is empty.");
            }

            if (input.field2 === undefined) {
                result.isValid = false;
                result.messages.push("field2 is empty.");
            }

            return result;
        }
        
        /* +++++ public interface +++++ */

        appService.logger.creation(componentName);

        return {
            validate: _validate
        };

    };

    theComponent.$inject = ['AppService'];

    angular.module('theApp').factory(componentName, theComponent);

})();