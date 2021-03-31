import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingCircularFormComponent } from './incoming-circular-form.component';

describe('IncomingCircularFormComponent', () => {
  let component: IncomingCircularFormComponent;
  let fixture: ComponentFixture<IncomingCircularFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingCircularFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingCircularFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
