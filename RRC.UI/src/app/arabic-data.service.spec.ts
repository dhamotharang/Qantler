import { TestBed } from '@angular/core/testing';

import { ArabicDataService } from './arabic-data.service';

describe('ArabicDataService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ArabicDataService = TestBed.get(ArabicDataService);
    expect(service).toBeTruthy();
  });
});
