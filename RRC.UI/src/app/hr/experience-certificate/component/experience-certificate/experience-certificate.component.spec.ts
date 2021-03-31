import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExperienceCertificateComponent } from './experience-certificate.component';

describe('ExperienceCertificateComponent', () => {
  let component: ExperienceCertificateComponent;
  let fixture: ComponentFixture<ExperienceCertificateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExperienceCertificateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExperienceCertificateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
