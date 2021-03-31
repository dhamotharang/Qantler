import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalReportModalComponent } from './legal-report-modal.component';

describe('LegalReportModalComponent', () => {
  let component: LegalReportModalComponent;
  let fixture: ComponentFixture<LegalReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegalReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegalReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
