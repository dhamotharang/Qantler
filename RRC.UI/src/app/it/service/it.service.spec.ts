import { TestBed } from '@angular/core/testing';

import { ItService } from './it.service';

describe('ItService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ItService = TestBed.get(ItService);
    expect(service).toBeTruthy();
  });
});
