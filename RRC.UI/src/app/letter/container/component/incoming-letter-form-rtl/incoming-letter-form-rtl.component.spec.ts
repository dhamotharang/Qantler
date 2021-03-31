import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingLetterFormRtlComponent } from './incoming-letter-form-rtl.component';

describe('IncomingLetterFormRtlComponent', () => {
  let component: IncomingLetterFormRtlComponent;
  let fixture: ComponentFixture<IncomingLetterFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingLetterFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingLetterFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
