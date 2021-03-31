import { TestBed } from '@angular/core/testing';

import { CalendarManagementService } from './calendar-management.service';

describe('CalendarManagementService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CalendarManagementService = TestBed.get(CalendarManagementService);
    expect(service).toBeTruthy();
  });
});
