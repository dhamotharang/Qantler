import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestPhotoRtlComponent } from './media-request-photo-rtl.component';

describe('MediaRequestPhotoRtlComponent', () => {
  let component: MediaRequestPhotoRtlComponent;
  let fixture: ComponentFixture<MediaRequestPhotoRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestPhotoRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestPhotoRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
