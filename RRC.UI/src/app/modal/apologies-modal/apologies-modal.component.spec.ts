import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApologiesModalComponent } from './apologies-modal.component';

describe('ApologiesModalComponent', () => {
  let component: ApologiesModalComponent;
  let fixture: ComponentFixture<ApologiesModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApologiesModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApologiesModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
