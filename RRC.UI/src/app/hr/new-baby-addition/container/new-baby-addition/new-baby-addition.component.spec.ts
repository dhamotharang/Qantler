import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBabyAdditionComponent } from './new-baby-addition.component';

describe('NewBabyAdditionComponent', () => {
  let component: NewBabyAdditionComponent;
  let fixture: ComponentFixture<NewBabyAdditionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewBabyAdditionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewBabyAdditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
