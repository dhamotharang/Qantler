import { TestBed } from '@angular/core/testing';

import { MediapressService } from './mediapress.service';

describe('MediapressService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MediapressService = TestBed.get(MediapressService);
    expect(service).toBeTruthy();
  });
});
