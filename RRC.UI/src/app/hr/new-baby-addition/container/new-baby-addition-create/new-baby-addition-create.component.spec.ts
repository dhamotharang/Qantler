import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBabyAdditionCreateComponent } from './new-baby-addition-create.component';

describe('NewBabyAdditionCreateComponent', () => {
  let component: NewBabyAdditionCreateComponent;
  let fixture: ComponentFixture<NewBabyAdditionCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewBabyAdditionCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewBabyAdditionCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
