import { TestBed } from '@angular/core/testing';

import { StorageUploadService } from './storage-upload.service';

describe('StorageUploadService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StorageUploadService = TestBed.get(StorageUploadService);
    expect(service).toBeTruthy();
  });
});
