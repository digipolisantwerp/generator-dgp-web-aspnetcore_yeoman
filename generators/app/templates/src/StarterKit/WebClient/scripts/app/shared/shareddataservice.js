(function () {
	'use strict';
	var componentName = "SharedDataService";
	var theComponent = function (appService) {
		appService.logger.creation(componentName);
		
        var customerDetails = {};

		function _setCustomerDetails(data) {
			customerDetails = data;
		}
		function _getCustomerDetails() {
			return customerDetails;
		}

		return {
			setCustomerDetails: _setCustomerDetails,
			getCustomerDetails: _getCustomerDetails
		};
	};
	theComponent.$inject = ['AppService'];
	angular.module('theApp').factory(componentName, theComponent);
})();
