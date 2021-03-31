import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalaryCertificateViewComponent } from './salary-certificate-view.component';

describe('SalaryCertificateViewComponent', () => {
  let component: SalaryCertificateViewComponent;
  let fixture: ComponentFixture<SalaryCertificateViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalaryCertificateViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalaryCertificateViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
