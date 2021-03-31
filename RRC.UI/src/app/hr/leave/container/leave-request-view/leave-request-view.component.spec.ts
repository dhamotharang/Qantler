import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveRequestViewComponent } from './leave-request-view.component';

describe('LeaveRequestViewComponent', () => {
  let component: LeaveRequestViewComponent;
  let fixture: ComponentFixture<LeaveRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LeaveRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaveRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
