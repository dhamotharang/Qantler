import { TestBed } from '@angular/core/testing';

import { LeaveRequestService } from './leave-request.service';

describe('LeaveRequestService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LeaveRequestService = TestBed.get(LeaveRequestService);
    expect(service).toBeTruthy();
  });
});
