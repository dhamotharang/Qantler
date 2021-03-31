import { TestBed } from '@angular/core/testing';

import { MdmServiceService } from './mdm-service.service';

describe('MdmServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MdmServiceService = TestBed.get(MdmServiceService);
    expect(service).toBeTruthy();
  });
});
