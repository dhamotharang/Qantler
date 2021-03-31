import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReturnFormContainerComponent } from './vehicle-return-form-container.component';

describe('VehicleReturnFormContainerComponent', () => {
  let component: VehicleReturnFormContainerComponent;
  let fixture: ComponentFixture<VehicleReturnFormContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReturnFormContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReturnFormContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
