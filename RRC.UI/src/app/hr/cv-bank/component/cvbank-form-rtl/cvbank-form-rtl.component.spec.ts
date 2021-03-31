import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvbankFormRtlComponent } from './cvbank-form-rtl.component';

describe('CvbankFormRtlComponent', () => {
  let component: CvbankFormRtlComponent;
  let fixture: ComponentFixture<CvbankFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvbankFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvbankFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
