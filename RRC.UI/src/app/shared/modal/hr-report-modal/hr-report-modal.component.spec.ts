import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrReportModalComponent } from './hr-report-modal.component';

describe('HrReportModalComponent', () => {
  let component: HrReportModalComponent;
  let fixture: ComponentFixture<HrReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
