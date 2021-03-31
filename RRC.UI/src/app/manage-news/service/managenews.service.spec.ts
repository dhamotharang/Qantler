import { TestBed } from '@angular/core/testing';

import { ManagenewsService } from './managenews.service';

describe('ManagenewsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ManagenewsService = TestBed.get(ManagenewsService);
    expect(service).toBeTruthy();
  });
});
