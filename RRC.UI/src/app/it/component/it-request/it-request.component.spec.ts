import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItRequestComponent } from './it-request.component';

describe('ItRequestComponent', () => {
  let component: ItRequestComponent;
  let fixture: ComponentFixture<ItRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
