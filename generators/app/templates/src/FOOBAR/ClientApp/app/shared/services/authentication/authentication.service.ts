import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {UserModel} from '../../models/userModel';
import {HttpBase} from '../../helpers/httpBase.class';
import {WebStorageService} from '../web-storage/web-storage.service';
import {WebStorageKeysEnum} from '../../enums/web-storage-keys.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends HttpBase {

  get cachedPermissions() {
    return this._webStorageService.getSessionStorage(WebStorageKeysEnum.PERMISSIONS);
  }

  constructor(
    private _client: HttpClient,
    private _webStorageService: WebStorageService
  ) {
    super();
  }

  hasPermission(permission: string): Observable<boolean> {
    const url = `${this.bffApiUrl}/authorization/haspermission/?permission=${permission}`;
    return this._client.get<boolean>(url);
  }

  hasPermissionIn(permissions: Array<string>): Observable<boolean> {
    permissions = permissions.map((permission: string) => {
      return `permissions=${permission}`;
    });

    const url = `${this.bffApiUrl}/authorization/haspermissionin/?${permissions.join('&')}`;
    return this._client.get<boolean>(url);
  }

  logout(): Observable<any> {
    this._webStorageService.clearSessionStorage();

    const url = `${this.bffApiUrl}/user/logout`;
    return this._client.get<any>(url);
  }

  getCurrentUser(): Observable<UserModel> {
    const url = `${this.bffApiUrl}/user`;
    return this._client.get<UserModel>(url);
  }
}

