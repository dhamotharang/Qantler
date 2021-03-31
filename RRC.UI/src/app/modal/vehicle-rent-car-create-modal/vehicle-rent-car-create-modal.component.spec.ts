import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleRentCarCreateModalComponent } from './vehicle-rent-car-create-modal.component';

describe('VehicleRentCarCreateModalComponent', () => {
  let component: VehicleRentCarCreateModalComponent;
  let fixture: ComponentFixture<VehicleRentCarCreateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleRentCarCreateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleRentCarCreateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
