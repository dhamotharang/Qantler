import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignModalComponent } from './assignmodal.component';

describe('AssignModalComponent', () => {
  let component: AssignModalComponent;
  let fixture: ComponentFixture<AssignModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
