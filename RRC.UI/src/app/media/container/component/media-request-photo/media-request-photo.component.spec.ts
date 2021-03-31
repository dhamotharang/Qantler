import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestPhotoComponent } from './media-request-photo.component';

describe('MediaRequestPhotoComponent', () => {
  let component: MediaRequestPhotoComponent;
  let fixture: ComponentFixture<MediaRequestPhotoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestPhotoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestPhotoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
