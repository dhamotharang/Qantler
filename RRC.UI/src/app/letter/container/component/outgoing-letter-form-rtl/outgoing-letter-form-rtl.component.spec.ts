import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OutgoingLetterFormRtlComponent } from './outgoing-letter-form-rtl.component';

describe('OutgoingLetterFormRtlComponent', () => {
  let component: OutgoingLetterFormRtlComponent;
  let fixture: ComponentFixture<OutgoingLetterFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutgoingLetterFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutgoingLetterFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
