import { TestBed } from '@angular/core/testing';

import { EncService } from './enc.service';

describe('EncService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EncService = TestBed.get(EncService);
    expect(service).toBeTruthy();
  });
});
