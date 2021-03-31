import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaPressReleaseEditComponent } from './media-press-release-edit.component';

describe('MediaPressReleaseEditComponent', () => {
  let component: MediaPressReleaseEditComponent;
  let fixture: ComponentFixture<MediaPressReleaseEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaPressReleaseEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaPressReleaseEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
