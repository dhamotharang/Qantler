import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleServiceLogComponent } from './vehicle-service-log.component';

describe('VehicleServiceLogComponent', () => {
  let component: VehicleServiceLogComponent;
  let fixture: ComponentFixture<VehicleServiceLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleServiceLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleServiceLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
