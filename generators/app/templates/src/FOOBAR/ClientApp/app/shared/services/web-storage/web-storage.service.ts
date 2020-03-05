import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebStorageService {

  setSessionStorage(key: string, value: any) {
    sessionStorage.setItem(key, JSON.stringify(value));
  }

  getSessionStorage(key: string) {
    try {
      const storage = sessionStorage.getItem(key);

      if (storage) {
        return JSON.parse(storage);
      }

      return null;
    } catch (error) {
      return null;
    }
  }

  clearSessionStorage() {
    sessionStorage.clear();
  }

  setLocalStorage(key: string, value: any) {
    localStorage.setItem(key, JSON.stringify(value));
  }

  getLocalStorage(key: string) {
    try {
      const storage = localStorage.getItem(key);

      if (storage) {
        return JSON.parse(storage);
      }

      return null;
    } catch (error) {
      return null;
    }
  }

  removeLocalStorage(key: string) {
    localStorage.removeItem(key);
  }
}
