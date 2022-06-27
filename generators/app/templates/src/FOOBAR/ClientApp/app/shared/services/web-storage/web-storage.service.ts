import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class WebStorageService {
  public hasSessionStorageKey(key: string): boolean {
    return sessionStorage.getItem(key) !== null;
  }

  public setSessionStorage(key: string, value: any) {
    sessionStorage.setItem(key, JSON.stringify(value));
  }

  public getSessionStorage(key: string) {
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

  public clearSessionStorage() {
    sessionStorage.clear();
  }

  public setLocalStorage(key: string, value: any) {
    localStorage.setItem(key, JSON.stringify(value));
  }

  public getLocalStorage(key: string) {
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

  public removeLocalStorage(key: string) {
    localStorage.removeItem(key);
  }
}
