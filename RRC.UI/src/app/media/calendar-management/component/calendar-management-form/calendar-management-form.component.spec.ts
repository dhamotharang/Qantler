import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarManagementFormComponent } from './calendar-management-form.component';

describe('CalendarManagementFormComponent', () => {
  let component: CalendarManagementFormComponent;
  let fixture: ComponentFixture<CalendarManagementFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalendarManagementFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarManagementFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
