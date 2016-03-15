describe("Example of a test spec", function () {

	var $compile, $rootScope;

	beforeEach(module('theApp'));

	beforeEach(inject(function (_$compile_, _$rootScope_) {
		$compile = _$compile_;
		$rootScope = _$rootScope_;
	}));

	it("Sorts a list", function () {
		var list = ["pear", "carrot", "banana"];
		var sorted = sort(list);
		expect(sorted).toEqual(["banana", "carrot", "pear"]);
	});

});

function sort(input) {
	return input.sort();
}