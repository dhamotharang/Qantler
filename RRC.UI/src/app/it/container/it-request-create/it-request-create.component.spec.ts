import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItRequestCreateComponent } from './it-request-create.component';

describe('ItRequestCreateComponent', () => {
  let component: ItRequestCreateComponent;
  let fixture: ComponentFixture<ItRequestCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItRequestCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
