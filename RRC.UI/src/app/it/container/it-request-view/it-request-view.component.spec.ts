import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItRequestViewComponent } from './it-request-view.component';

describe('ItRequestViewComponent', () => {
  let component: ItRequestViewComponent;
  let fixture: ComponentFixture<ItRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
