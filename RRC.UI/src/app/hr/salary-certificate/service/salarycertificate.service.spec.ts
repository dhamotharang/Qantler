import { TestBed } from '@angular/core/testing';

import { SalarycertificateService } from './salarycertificate.service';

describe('SalarycertificateService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SalarycertificateService = TestBed.get(SalarycertificateService);
    expect(service).toBeTruthy();
  });
});
