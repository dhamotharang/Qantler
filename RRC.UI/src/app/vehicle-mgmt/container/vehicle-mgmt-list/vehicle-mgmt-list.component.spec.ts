import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleMgmtListComponent } from './vehicle-mgmt-list.component';

describe('VehicleMgmtListComponent', () => {
  let component: VehicleMgmtListComponent;
  let fixture: ComponentFixture<VehicleMgmtListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleMgmtListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleMgmtListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
