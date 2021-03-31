import { TestBed } from '@angular/core/testing';

import { ManagePhotoService } from './manage-photo.service';

describe('ManagePhotoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ManagePhotoService = TestBed.get(ManagePhotoService);
    expect(service).toBeTruthy();
  });
});
