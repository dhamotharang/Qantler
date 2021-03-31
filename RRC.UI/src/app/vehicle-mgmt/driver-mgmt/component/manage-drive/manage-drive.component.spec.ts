import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDriveComponent } from './manage-drive.component';

describe('ManageDriveComponent', () => {
  let component: ManageDriveComponent;
  let fixture: ComponentFixture<ManageDriveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDriveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDriveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
