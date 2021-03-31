import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaPressReleaseContainerComponent } from './media-press-release-container.component';

describe('MediaPressReleaseContainerComponent', () => {
  let component: MediaPressReleaseContainerComponent;
  let fixture: ComponentFixture<MediaPressReleaseContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaPressReleaseContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaPressReleaseContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
