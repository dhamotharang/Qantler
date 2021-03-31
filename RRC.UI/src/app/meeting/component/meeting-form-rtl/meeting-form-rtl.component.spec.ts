import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingFormRtlComponent } from './meeting-form-rtl.component';

describe('MeetingFormRtlComponent', () => {
  let component: MeetingFormRtlComponent;
  let fixture: ComponentFixture<MeetingFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MeetingFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MeetingFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
