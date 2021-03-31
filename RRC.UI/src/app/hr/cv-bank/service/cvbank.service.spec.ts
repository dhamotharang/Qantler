import { TestBed } from '@angular/core/testing';

import { CvbankService } from './cvbank.service';

describe('CvbankService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CvbankService = TestBed.get(CvbankService);
    expect(service).toBeTruthy();
  });
});
