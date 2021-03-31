import { TestBed } from '@angular/core/testing';

import { NewBabyAdditionService } from './new-baby-addition.service';

describe('NewBabyAdditionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NewBabyAdditionService = TestBed.get(NewBabyAdditionService);
    expect(service).toBeTruthy();
  });
});
