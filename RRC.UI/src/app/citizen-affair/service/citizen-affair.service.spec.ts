import { TestBed } from '@angular/core/testing';

import { CitizenAffairService } from './citizen-affair.service';

describe('CitizenAffairService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CitizenAffairService = TestBed.get(CitizenAffairService);
    expect(service).toBeTruthy();
  });
});
