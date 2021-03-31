import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarManagementFormRtlComponent } from './calendar-management-form-rtl.component';

describe('CalendarManagementFormRtlComponent', () => {
  let component: CalendarManagementFormRtlComponent;
  let fixture: ComponentFixture<CalendarManagementFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarManagementFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarManagementFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
