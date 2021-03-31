import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReturnConfirmationComponent } from './vehicle-return-confirmation.component';

describe('VehicleReturnConfirmationComponent', () => {
  let component: VehicleReturnConfirmationComponent;
  let fixture: ComponentFixture<VehicleReturnConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReturnConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReturnConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
