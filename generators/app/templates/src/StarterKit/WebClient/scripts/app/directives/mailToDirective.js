(function () {
    'use strict';
    var directiveName = "mailTo";
    var theDirective = function () {
        return {
            restrict: 'E',
            replace: true,           
            template:
                '<a href="mailto:{{emailAddress}}">{{emailAddress}}</a>',
            scope: {
                emailAdres: "="
            }
        }
    }

    angular.module('theApp').directive(directiveName, theDirective);
})();