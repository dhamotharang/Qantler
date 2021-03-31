import { TestBed } from '@angular/core/testing';

import { AbuseReportService } from './abuse-report.service';

describe('AbuseReportService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AbuseReportService = TestBed.get(AbuseReportService);
    expect(service).toBeTruthy();
  });
});
