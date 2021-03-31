import { async, inject, TestBed } from '@angular/core/testing';
import { ProfileUpdateGuard } from './profile-update.guard';

describe('ProfileUpdateGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProfileUpdateGuard]
    });
  });

  it('should ...', inject([ProfileUpdateGuard], (guard: ProfileUpdateGuard) => {
    expect(guard).toBeTruthy();
  }));
});
