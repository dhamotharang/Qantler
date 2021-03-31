import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleDetailsViewComponent } from './vehicle-details-view.component';

describe('VehicleDetailsViewComponent', () => {
  let component: VehicleDetailsViewComponent;
  let fixture: ComponentFixture<VehicleDetailsViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleDetailsViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleDetailsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
