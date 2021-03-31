import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CvbankListComponent } from './cvbank-list.component';

describe('CvbankListComponent', () => {
  let component: CvbankListComponent;
  let fixture: ComponentFixture<CvbankListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CvbankListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CvbankListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
