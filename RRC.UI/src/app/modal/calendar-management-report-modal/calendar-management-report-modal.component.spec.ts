import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarManagementReportModalComponent } from './calendar-management-report-modal.component';

describe('CalendarManagementReportModalComponent', () => {
  let component: CalendarManagementReportModalComponent;
  let fixture: ComponentFixture<CalendarManagementReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarManagementReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarManagementReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
