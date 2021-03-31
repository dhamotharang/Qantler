import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReleaseFormComponent } from './vehicle-release-form.component';

describe('VehicleReleaseFormComponent', () => {
  let component: VehicleReleaseFormComponent;
  let fixture: ComponentFixture<VehicleReleaseFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReleaseFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReleaseFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
