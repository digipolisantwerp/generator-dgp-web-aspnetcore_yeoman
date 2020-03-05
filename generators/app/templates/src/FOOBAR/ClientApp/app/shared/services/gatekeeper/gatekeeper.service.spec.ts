import { TestBed } from '@angular/core/testing';

import { GatekeeperService } from './gate-keeper.service';

describe('GateKeeperService', () => {
  let service: GatekeeperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GatekeeperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
