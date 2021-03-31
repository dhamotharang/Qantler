import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeProfileCreateComponent } from './employee-profile-create.component';

describe('EmployeeProfileCreateComponent', () => {
  let component: EmployeeProfileCreateComponent;
  let fixture: ComponentFixture<EmployeeProfileCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeProfileCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeProfileCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
