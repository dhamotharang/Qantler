import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewBabyAdditionViewComponent } from './new-baby-addition-view.component';

describe('NewBabyAdditionViewComponent', () => {
  let component: NewBabyAdditionViewComponent;
  let fixture: ComponentFixture<NewBabyAdditionViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewBabyAdditionViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewBabyAdditionViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
