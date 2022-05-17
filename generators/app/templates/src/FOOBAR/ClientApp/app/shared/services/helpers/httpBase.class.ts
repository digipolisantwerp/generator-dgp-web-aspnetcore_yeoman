import { environment } from '../../../../environments/environment';

export class HttpBase {
  protected apiUrl: string;
  protected bffApiUrl: string;

  constructor() {
    this.apiUrl = environment.passThroughApiUrl;
    this.bffApiUrl = environment.bffApiUrl;
  }
}
