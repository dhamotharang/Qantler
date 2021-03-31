import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItReportModalComponent } from './it-report-modal.component';

describe('ItReportModalComponent', () => {
  let component: ItReportModalComponent;
  let fixture: ComponentFixture<ItReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
