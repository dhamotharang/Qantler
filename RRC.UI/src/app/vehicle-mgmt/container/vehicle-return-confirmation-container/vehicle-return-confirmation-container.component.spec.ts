import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReturnConfirmationContainerComponent } from './vehicle-return-confirmation-container.component';

describe('VehicleReturnConfirmationContainerComponent', () => {
  let component: VehicleReturnConfirmationContainerComponent;
  let fixture: ComponentFixture<VehicleReturnConfirmationContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReturnConfirmationContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReturnConfirmationContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
