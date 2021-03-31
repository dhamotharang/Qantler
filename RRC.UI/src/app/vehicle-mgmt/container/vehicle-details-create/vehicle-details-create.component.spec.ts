import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleDetailsCreateComponent } from './vehicle-details-create.component';

describe('VehicleDetailsCreateComponent', () => {
  let component: VehicleDetailsCreateComponent;
  let fixture: ComponentFixture<VehicleDetailsCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleDetailsCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleDetailsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
