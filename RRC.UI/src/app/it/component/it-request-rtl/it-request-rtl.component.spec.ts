import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItRequestRtlComponent } from './it-request-rtl.component';

describe('ItRequestRtlComponent', () => {
  let component: ItRequestRtlComponent;
  let fixture: ComponentFixture<ItRequestRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItRequestRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItRequestRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
