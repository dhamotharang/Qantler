import { TestBed, async, inject } from '@angular/core/testing';

import { BrowserCheckGuard } from './browser-check.guard';

describe('BrowserCheckGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BrowserCheckGuard]
    });
  });

  it('should ...', inject([BrowserCheckGuard], (guard: BrowserCheckGuard) => {
    expect(guard).toBeTruthy();
  }));
});
