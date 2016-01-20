describe("Voorbeeldje van test spec", function () {

	var $compile, $rootScope;

	beforeEach(module('theApp'));

	beforeEach(inject(function (_$compile_, _$rootScope_) {
		$compile = _$compile_;
		$rootScope = _$rootScope_;
	}));

	it("Sorteert een lijst", function () {
		var lijst = ["peer", "wortel", "banaan"];
		var gesorteerd = sorteer(lijst);
		expect(gesorteerd).toEqual(["banaan", "peer", "wortel"]);
	});

});

function sorteer(input) {
	return input.sort();
}