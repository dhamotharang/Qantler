import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QrcReportsComponent } from './qrc-reports.component';

describe('QrcReportsComponent', () => {
  let component: QrcReportsComponent;
  let fixture: ComponentFixture<QrcReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QrcReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QrcReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
