import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CircularListComponent } from './circular-list.component';

describe('CircularListComponent', () => {
  let component: CircularListComponent;
  let fixture: ComponentFixture<CircularListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CircularListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CircularListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
