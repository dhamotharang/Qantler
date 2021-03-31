import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleMgmtComponent } from './vehicle-mgmt.component';

describe('VehicleMgmtComponent', () => {
  let component: VehicleMgmtComponent;
  let fixture: ComponentFixture<VehicleMgmtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleMgmtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleMgmtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
