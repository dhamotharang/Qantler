import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EscalateModalComponent } from './escalate-modal.component';

describe('EscalateModalComponent', () => {
  let component: EscalateModalComponent;
  let fixture: ComponentFixture<EscalateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EscalateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EscalateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
