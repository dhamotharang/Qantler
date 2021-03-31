import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleRequestContainerComponent } from './vehicle-request-container.component';

describe('VehicleRequestContainerComponent', () => {
  let component: VehicleRequestContainerComponent;
  let fixture: ComponentFixture<VehicleRequestContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleRequestContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleRequestContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
