import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleMgmtAssignComponent } from './vehicle-mgmt-assign.component';

describe('VehicleMgmtAssignComponent', () => {
  let component: VehicleMgmtAssignComponent;
  let fixture: ComponentFixture<VehicleMgmtAssignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleMgmtAssignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleMgmtAssignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
