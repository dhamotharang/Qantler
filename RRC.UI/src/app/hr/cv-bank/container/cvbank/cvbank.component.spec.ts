import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvbankComponent } from './cvbank.component';

describe('CvbankComponent', () => {
  let component: CvbankComponent;
  let fixture: ComponentFixture<CvbankComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvbankComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvbankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
