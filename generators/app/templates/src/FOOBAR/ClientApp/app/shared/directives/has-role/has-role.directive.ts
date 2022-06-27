import {
  Directive,
  Input,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from "@angular/core";
import { AuthenticationService } from "../../services/authentication/authentication.service";

@Directive({
  selector: "[appHasRole]",
})
export class HasRoleDirective implements OnInit, OnDestroy {
  @Input() appHasRole: Array<string>;

  private hasRoleSubscription;

  constructor(
    private _viewContainerRef: ViewContainerRef,
    private _templateRef: TemplateRef<any>,
    private _authenticationService: AuthenticationService
  ) {}

  public ngOnInit(): void {
    if (this._authenticationService.cachedPermissions) {
      this.appHasRole.some((item) => {
        if (this._authenticationService.cachedPermissions.includes(item)) {
          this.show();
        } else {
          this.hide();
        }
      });
    } else {
      this._authenticationService
        .hasPermissionIn(this.appHasRole)
        .subscribe((response) => {
          if (response) {
            this.show();
          } else {
            this.hide();
          }
        });
    }
  }

  public ngOnDestroy(): void {
    if (this.hasRoleSubscription) {
      this.hasRoleSubscription.unsubscribe();
    }
  }

  private show() {
    this.hide();
    this._viewContainerRef.createEmbeddedView(this._templateRef);
  }

  private hide() {
    this._viewContainerRef.clear();
  }
}
