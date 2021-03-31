import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvbankFormComponent } from './cvbank-form.component';

describe('CvbankFormComponent', () => {
  let component: CvbankFormComponent;
  let fixture: ComponentFixture<CvbankFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvbankFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvbankFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
