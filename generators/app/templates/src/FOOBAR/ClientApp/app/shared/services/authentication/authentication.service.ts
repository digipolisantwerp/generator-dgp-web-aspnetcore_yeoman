import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { UserModel } from "../../models/userModel";
import { HttpBase } from "../../helpers/httpBase.class";
import { WebStorageService } from "../web-storage/web-storage.service";
import { WebStorageKeysEnum } from "../../enums/web-storage-keys.enum";
import { map, tap } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class AuthenticationService extends HttpBase {
  public get cachedPermissions() {
    return this._webStorageService.getSessionStorage(
      WebStorageKeysEnum.PERMISSIONS
    );
  }

  constructor(
    private _client: HttpClient,
    private _webStorageService: WebStorageService
  ) {
    super();
  }

  public hasPermission(permission: string): Observable<boolean> {
    const url = `${this.bffApiUrl}/authorization/haspermission/?permission=${permission}`;
    return this._client.get<boolean>(url);
  }

  public hasPermissionIn(permissions: Array<string>): Observable<boolean> {
    return this.getPermissions().pipe(
      map((perms: string[]) => {
        return permissions.some((item) => {
          return perms.includes(item);
        });
      })
    );
  }

  public logout(): Observable<any> {
    this._webStorageService.clearSessionStorage();

    const url = `${this.bffApiUrl}/user/logout`;
    return this._client.get<any>(url);
  }

  public getCurrentUser(): Observable<UserModel> {
    const url = `${this.bffApiUrl}/user`;
    return this._client.get<UserModel>(url);
  }

  private setCachedPermissions(permissions: string[]): void {
    this._webStorageService.setSessionStorage(
      WebStorageKeysEnum.PERMISSIONS,
      permissions
    );
  }

  private getPermissions(): Observable<Array<string>> {
    if (
      this._webStorageService.hasSessionStorageKey(
        WebStorageKeysEnum.PERMISSIONS
      )
    ) {
      return of(this.cachedPermissions);
    }

    const url = `${this.bffApiUrl}/authorization/permissions`;
    return this._client
      .get<Array<string>>(url)
      .pipe(
        tap((permissions: Array<string>) =>
          this.setCachedPermissions(permissions)
        )
      );
  }
}
