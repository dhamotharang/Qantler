import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReleaseFormContainerComponent } from './vehicle-release-form-container.component';

describe('VehicleReleaseFormContainerComponent', () => {
  let component: VehicleReleaseFormContainerComponent;
  let fixture: ComponentFixture<VehicleReleaseFormContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReleaseFormContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReleaseFormContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
