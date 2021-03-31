import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleRequestViewComponent } from './vehicle-request-view.component';

describe('VehicleRequestViewComponent', () => {
  let component: VehicleRequestViewComponent;
  let fixture: ComponentFixture<VehicleRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
