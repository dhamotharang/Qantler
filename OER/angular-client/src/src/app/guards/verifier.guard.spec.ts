import { TestBed, async, inject } from '@angular/core/testing';

import { VerifierGuard } from './verifier.guard';

describe('VerifierGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [VerifierGuard]
    });
  });

  it('should ...', inject([VerifierGuard], (guard: VerifierGuard) => {
    expect(guard).toBeTruthy();
  }));
});
