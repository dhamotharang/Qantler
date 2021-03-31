import { TestBed } from '@angular/core/testing';

import { GiftsManagementService } from './gifts-management.service';

describe('GiftsManagementService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GiftsManagementService = TestBed.get(GiftsManagementService);
    expect(service).toBeTruthy();
  });
});
