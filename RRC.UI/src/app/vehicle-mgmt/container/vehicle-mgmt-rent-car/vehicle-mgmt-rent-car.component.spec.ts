import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleMgmtRentCarComponent } from './vehicle-mgmt-rent-car.component';

describe('VehicleMgmtRentCarComponent', () => {
  let component: VehicleMgmtRentCarComponent;
  let fixture: ComponentFixture<VehicleMgmtRentCarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleMgmtRentCarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleMgmtRentCarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
