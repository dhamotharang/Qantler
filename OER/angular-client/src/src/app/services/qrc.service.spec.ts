import { TestBed } from '@angular/core/testing';

import { QrcService } from './qrc.service';

describe('QrcService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: QrcService = TestBed.get(QrcService);
    expect(service).toBeTruthy();
  });
});
