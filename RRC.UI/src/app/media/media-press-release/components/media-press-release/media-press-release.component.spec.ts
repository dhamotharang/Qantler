import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaPressReleaseComponent } from './media-press-release.component';

describe('MediaPressReleaseComponent', () => {
  let component: MediaPressReleaseComponent;
  let fixture: ComponentFixture<MediaPressReleaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaPressReleaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaPressReleaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
