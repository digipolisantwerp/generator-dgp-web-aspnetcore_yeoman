describe("Tests for mailToDirective", function () {

	var $compile, $rootScope;

	beforeEach(module('theApp'));

	beforeEach(inject(function (_$compile_, _$rootScope_, $httpBackend) {
		$compile = _$compile_;
		$rootScope = _$rootScope_;
		$httpBackend.whenGET(/views/).respond(200, '');
	}));

	it("inserts an anchor tag", function () {
		var wrappedElement = $compile("<div><mail-to email-address='myAddress'></mail-to></div>")($rootScope);
		$rootScope.$digest();
		expect(wrappedElement.html()).toContain("<a href=");
	});
	
	it("href contains mailTo + e-mail address", function () {
		$rootScope.myAddress = "me@somewhere.com";
		var element = $compile("<mail-to email-address='myAddress'></mail-to>")($rootScope);
		$rootScope.$digest();
		expect(element.attr("href")).toContain("mailto:me@somewhere.com");
	});

	it("anchor has e-mail address as display text", function () {
		$rootScope.myAddress = "me@somewhere.com";
		var element = $compile("<mail-to email-address='myAddress'></mail-to>")($rootScope);
		$rootScope.$digest();
		expect(element.text()).toContain("me@somewhere.com");
	});

});