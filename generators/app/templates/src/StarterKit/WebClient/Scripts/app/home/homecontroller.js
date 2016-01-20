(function () {
	'use strict';
	var ctrlName = "HomeController";
	var theController = function (appService) {

		appService.logger.creation(ctrlName);

		var ctrl = this;
		ctrl.titel = "StarterKit";

		init();

		function init() {
			appService.logger.init(ctrlName);
		}
	};

	theController.$inject = ['AppService'];
	angular.module('theApp').controller(ctrlName, theController);
})();