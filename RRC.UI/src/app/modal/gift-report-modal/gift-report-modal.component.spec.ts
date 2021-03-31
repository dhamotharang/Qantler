import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftReportModalComponent } from './gift-report-modal.component';

describe('GiftReportModalComponent', () => {
  let component: GiftReportModalComponent;
  let fixture: ComponentFixture<GiftReportModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftReportModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftReportModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
