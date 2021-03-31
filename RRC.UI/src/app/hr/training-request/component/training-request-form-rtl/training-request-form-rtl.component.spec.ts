import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingRequestFormRtlComponent } from './training-request-form-rtl.component';

describe('TrainingRequestFormRtlComponent', () => {
  let component: TrainingRequestFormRtlComponent;
  let fixture: ComponentFixture<TrainingRequestFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingRequestFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingRequestFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
