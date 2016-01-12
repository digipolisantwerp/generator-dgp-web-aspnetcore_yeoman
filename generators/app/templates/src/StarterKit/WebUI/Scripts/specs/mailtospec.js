describe("Testen voor mailToDirective", function () {

	var $compile, $rootScope;

	beforeEach(module('theApp'));

	beforeEach(inject(function (_$compile_, _$rootScope_, $httpBackend) {
		$compile = _$compile_;
		$rootScope = _$rootScope_;
		$httpBackend.whenGET(/views/).respond(200, '');
	}));

	it("geeft een anchor tag", function () {
		var wrappedElement = $compile("<div><mail-to email-adres='mijnadres'></mail-to></div>")($rootScope);
		$rootScope.$digest();
		expect(wrappedElement.html()).toContain("<a href=");
	});
	
	it("href bevat mailto + e-mailadres", function () {
		$rootScope.mijnadres = "ikke@ergens.be";
		var element = $compile("<mail-to email-adres='mijnadres'></mail-to>")($rootScope);
		$rootScope.$digest();
		expect(element.attr("href")).toContain("mailto:ikke@ergens.be");
	});

	it("anchor heeft e-mail adres als display tekst", function () {
		$rootScope.mijnadres = "ikke@ergens.be";
		var element = $compile("<mail-to email-adres='mijnadres'></mail-to>")($rootScope);
		$rootScope.$digest();
		expect(element.text()).toContain("ikke@ergens.be");
	});

	//it("geen e-mail adres geeft span", function () {
	//	var wrappedElement = $compile("<div><mail-to email-adres='mijnadres'></mail-to></div>")($rootScope);
	//	$rootScope.$digest();
	//	expect(wrappedElement.html()).toContain("<span />");
	//});

});