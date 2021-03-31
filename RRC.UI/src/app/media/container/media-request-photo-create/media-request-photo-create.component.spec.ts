import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestPhotoCreateComponent } from './media-request-photo-create.component';

describe('MediaRequestPhotoCreateComponent', () => {
  let component: MediaRequestPhotoCreateComponent;
  let fixture: ComponentFixture<MediaRequestPhotoCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestPhotoCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestPhotoCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
