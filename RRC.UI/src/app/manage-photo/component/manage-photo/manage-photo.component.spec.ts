import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagePhotoComponent } from './manage-photo.component';

describe('ManagePhotoComponent', () => {
  let component: ManagePhotoComponent;
  let fixture: ComponentFixture<ManagePhotoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagePhotoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagePhotoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
