import {Injectable} from '@angular/core';
import {AuthenticationService} from '../authentication/authentication.service';
import {ActivatedRouteSnapshot, Router, RouterStateSnapshot} from '@angular/router';
import {catchError, map} from 'rxjs/operators';
import {of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GateKeeperService {

  constructor(
    private _authenticationService: AuthenticationService,
    private _router: Router
  ) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.checkCredentials(route.data.roles);
  }

  private checkCredentials(roles: Array<string>) {
    const cachedPermissions = this._authenticationService.cachedPermissions;

    if (cachedPermissions && cachedPermissions.length) {
      return roles.some((item) => {
        return cachedPermissions.includes(item);
      });
    } else {
      return this._authenticationService.hasPermissionIn(roles).pipe(
        map((hasPermission: boolean) => {
          if (hasPermission) {
            return true;
          } else {
            this._router.navigate(['/verboden-toegang']);
            return of(false);
          }
        }), catchError((err) => {
          this._router.navigate(['/verboden-toegang']);
          return of(false);
        })
      );
    }
  }
}
