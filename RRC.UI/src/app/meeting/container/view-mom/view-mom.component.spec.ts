import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewMomComponent } from './view-mom.component';

describe('ViewMomComponent', () => {
  let component: ViewMomComponent;
  let fixture: ComponentFixture<ViewMomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewMomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewMomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
