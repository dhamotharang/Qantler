import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBabyAdditionFormComponent } from './new-baby-addition-form.component';

describe('NewBabyAdditionFormComponent', () => {
  let component: NewBabyAdditionFormComponent;
  let fixture: ComponentFixture<NewBabyAdditionFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewBabyAdditionFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewBabyAdditionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
