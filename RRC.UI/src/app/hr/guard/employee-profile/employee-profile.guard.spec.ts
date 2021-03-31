import { TestBed, async, inject } from '@angular/core/testing';

import { EmployeeProfileGuard } from './employee-profile.guard';

describe('EmployeeProfileGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EmployeeProfileGuard]
    });
  });

  it('should ...', inject([EmployeeProfileGuard], (guard: EmployeeProfileGuard) => {
    expect(guard).toBeTruthy();
  }));
});
