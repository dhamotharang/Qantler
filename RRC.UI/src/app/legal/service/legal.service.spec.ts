import { TestBed } from '@angular/core/testing';

import { LegalService } from './legal.service';

describe('LegalService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LegalService = TestBed.get(LegalService);
    expect(service).toBeTruthy();
  });
});
