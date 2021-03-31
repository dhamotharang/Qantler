import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveRequestFormRtlComponent } from './leave-request-form-rtl.component';

describe('LeaveRequestFormRtlComponent', () => {
  let component: LeaveRequestFormRtlComponent;
  let fixture: ComponentFixture<LeaveRequestFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeaveRequestFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaveRequestFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
