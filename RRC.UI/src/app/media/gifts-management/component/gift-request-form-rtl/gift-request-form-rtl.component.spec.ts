import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftRequestFormRtlComponent } from './gift-request-form-rtl.component';

describe('GiftRequestFormRtlComponent', () => {
  let component: GiftRequestFormRtlComponent;
  let fixture: ComponentFixture<GiftRequestFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftRequestFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftRequestFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
