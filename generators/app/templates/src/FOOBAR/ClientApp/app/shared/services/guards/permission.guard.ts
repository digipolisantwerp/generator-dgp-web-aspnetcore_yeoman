import { Injectable } from "@angular/core";
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from "@angular/router";
import { Observable, of } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { AuthenticationService } from "../authentication/authentication.service";

@Injectable({
  providedIn: "root",
})
export class PermissionGuard implements CanActivate {
  constructor(
    private _authenticationService: AuthenticationService,
    private _router: Router
  ) {}

  public canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | boolean {
    return this.checkCredentials(route.data.roles);
  }

  private checkCredentials(
    roles: Array<string>
  ): Observable<boolean> | boolean {
    return this._authenticationService.hasPermissionIn(roles).pipe(
      map((result: boolean) => {
        if (result) {
          return true;
        } else {
          this._router.navigate(["/verboden-toegang"]);
          return false;
        }
      }),
      catchError((err) => {
        this._router.navigate(["/verboden-toegang"]);
        return of(false);
      })
    );
  }
}
