import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaPressReleaseViewComponent } from './media-press-release-view.component';

describe('MediaPressReleaseViewComponent', () => {
  let component: MediaPressReleaseViewComponent;
  let fixture: ComponentFixture<MediaPressReleaseViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaPressReleaseViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaPressReleaseViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
