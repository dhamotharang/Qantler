import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifierReportsComponent } from './verifier-reports.component';

describe('VerifierReportsComponent', () => {
  let component: VerifierReportsComponent;
  let fixture: ComponentFixture<VerifierReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerifierReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifierReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
