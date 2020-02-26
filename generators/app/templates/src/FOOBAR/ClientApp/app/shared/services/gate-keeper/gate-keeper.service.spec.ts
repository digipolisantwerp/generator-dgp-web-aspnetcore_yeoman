import { TestBed } from '@angular/core/testing';

import { GateKeeperService } from './gate-keeper.service';

describe('GateKeeperService', () => {
  let service: GateKeeperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GateKeeperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
