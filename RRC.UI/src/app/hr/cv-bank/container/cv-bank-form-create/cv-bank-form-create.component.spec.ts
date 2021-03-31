import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvBankFormCreateComponent } from './cv-bank-form-create.component';

describe('CvBankFormCreateComponent', () => {
  let component: CvBankFormCreateComponent;
  let fixture: ComponentFixture<CvBankFormCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvBankFormCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvBankFormCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
