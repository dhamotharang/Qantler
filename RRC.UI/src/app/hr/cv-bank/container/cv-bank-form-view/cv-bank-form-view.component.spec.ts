import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvBankFormViewComponent } from './cv-bank-form-view.component';

describe('CvBankFormViewComponent', () => {
  let component: CvBankFormViewComponent;
  let fixture: ComponentFixture<CvBankFormViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvBankFormViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvBankFormViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
