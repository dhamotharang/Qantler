import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestStaffListComponent } from './media-request-staff-list.component';

describe('MediaRequestStaffListComponent', () => {
  let component: MediaRequestStaffListComponent;
  let fixture: ComponentFixture<MediaRequestStaffListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestStaffListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestStaffListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
