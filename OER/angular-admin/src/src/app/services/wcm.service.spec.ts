import { TestBed } from '@angular/core/testing';

import { WCMService } from './wcm.service';

describe('WCMService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WCMService = TestBed.get(WCMService);
    expect(service).toBeTruthy();
  });
});
