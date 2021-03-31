import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingRequestCreateComponent } from './training-request-create.component';

describe('TrainingRequestCreateComponent', () => {
  let component: TrainingRequestCreateComponent;
  let fixture: ComponentFixture<TrainingRequestCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrainingRequestCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrainingRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
