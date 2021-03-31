import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaPressReleaseRequestComponent } from './media-press-release-request.component';

describe('MediaPressReleaseRequestComponent', () => {
  let component: MediaPressReleaseRequestComponent;
  let fixture: ComponentFixture<MediaPressReleaseRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaPressReleaseRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaPressReleaseRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
