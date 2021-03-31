import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReturnFormComponent } from './vehicle-return-form.component';

describe('VehicleReturnFormComponent', () => {
  let component: VehicleReturnFormComponent;
  let fixture: ComponentFixture<VehicleReturnFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReturnFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReturnFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
