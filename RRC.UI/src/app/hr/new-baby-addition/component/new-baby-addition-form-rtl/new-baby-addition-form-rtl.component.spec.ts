import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBabyAdditionFormRtlComponent } from './new-baby-addition-form-rtl.component';

describe('NewBabyAdditionFormRtlComponent', () => {
  let component: NewBabyAdditionFormRtlComponent;
  let fixture: ComponentFixture<NewBabyAdditionFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewBabyAdditionFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewBabyAdditionFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
