import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LetterReportModalComponent } from './letter-report-modal.component';

describe('LetterReportModalComponent', () => {
  let component: LetterReportModalComponent;
  let fixture: ComponentFixture<LetterReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LetterReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LetterReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
