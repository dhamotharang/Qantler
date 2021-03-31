import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleRequestComponent } from './vehicle-request.component';

describe('VehicleRequestComponent', () => {
  let component: VehicleRequestComponent;
  let fixture: ComponentFixture<VehicleRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
