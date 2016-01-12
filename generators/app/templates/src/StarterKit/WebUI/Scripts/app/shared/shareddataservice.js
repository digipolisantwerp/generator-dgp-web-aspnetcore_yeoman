(function () {
	'use strict';
	var componentName = "SharedDataService";
	var theComponent = function (appService) {
		appService.logger.creation(componentName);
		var klantDetails = {};

		function _setKlantDetails(data) {
			klantDetails = data;
		}
		function _getKlantDetails() {
			return klantDetails;
		}

		return {
			setKlantDetails: _setKlantDetails,
			getKlantDetails: _getKlantDetails
		};
	};
	theComponent.$inject = ['AppService'];
	angular.module('theApp').factory(componentName, theComponent);
})();
