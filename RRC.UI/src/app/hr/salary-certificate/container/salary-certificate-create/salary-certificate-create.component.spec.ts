import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryCertificateCreateComponent } from './salary-certificate-create.component';

describe('SalaryCertificateCreateComponent', () => {
  let component: SalaryCertificateCreateComponent;
  let fixture: ComponentFixture<SalaryCertificateCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalaryCertificateCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryCertificateCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
