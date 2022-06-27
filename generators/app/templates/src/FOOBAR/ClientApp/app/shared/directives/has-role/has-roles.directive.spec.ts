import { HasRoleDirective } from "./has-role.directive";
import { TemplateRef, ViewContainerRef } from "@angular/core";
import { AuthenticationService } from "../../services/authentication/authentication.service";

describe("HasRoleDirective", () => {
  it("should create an instance", () => {
    const viewContainerRefMock: ViewContainerRef = null;
    const templateRefMock: TemplateRef<any> = null;

    const serviceMock: AuthenticationService = null;

    const directive = new HasRoleDirective(
      viewContainerRefMock,
      templateRefMock,
      serviceMock
    );
    expect(directive).toBeTruthy();
  });
});
