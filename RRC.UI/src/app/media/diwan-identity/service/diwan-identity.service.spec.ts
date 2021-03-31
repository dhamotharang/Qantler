import { TestBed } from '@angular/core/testing';

import { DiwanIdentityService } from './diwan-identity.service';

describe('DiwanIdentityService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DiwanIdentityService = TestBed.get(DiwanIdentityService);
    expect(service).toBeTruthy();
  });
});
