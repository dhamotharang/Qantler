import { TestBed } from '@angular/core/testing';

import { VehicleMgmtServiceService } from './vehicle-mgmt-service.service';

describe('VehicleMgmtServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: VehicleMgmtServiceService = TestBed.get(VehicleMgmtServiceService);
    expect(service).toBeTruthy();
  });
});
