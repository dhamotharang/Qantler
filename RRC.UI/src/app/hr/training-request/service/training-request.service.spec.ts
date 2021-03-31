import { TestBed } from '@angular/core/testing';

import { TrainingRequestService } from './training-request.service';

describe('TrainingRequestService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TrainingRequestService = TestBed.get(TrainingRequestService);
    expect(service).toBeTruthy();
  });
});
