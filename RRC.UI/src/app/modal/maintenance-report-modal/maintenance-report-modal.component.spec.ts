import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaintenanceReportModalComponent } from './maintenance-report-modal.component';

describe('MaintenanceReportModalComponent', () => {
  let component: MaintenanceReportModalComponent;
  let fixture: ComponentFixture<MaintenanceReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaintenanceReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaintenanceReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
