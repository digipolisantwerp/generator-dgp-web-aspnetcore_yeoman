// Voorbeeldje van een aparte component die kan geïnjecteerd worden in een controller.
// Deze kan gebruikt worden als template voor een validator component

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
                result.messages.push("Geen gegevens ingevuld.");
                return result;
            }

            if (input.field1 === undefined) {
                result.isValid = false;
                result.messages.push("field1 is niet ingevuld.");
            }

            if (input.field2 === undefined) {
                result.isValid = false;
                result.messages.push("field2 is niet ingevuld.");
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