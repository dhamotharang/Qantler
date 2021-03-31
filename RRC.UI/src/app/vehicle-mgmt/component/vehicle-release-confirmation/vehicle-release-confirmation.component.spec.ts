import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReleaseConfirmationComponent } from './vehicle-release-confirmation.component';

describe('VehicleReleaseConfirmationComponent', () => {
  let component: VehicleReleaseConfirmationComponent;
  let fixture: ComponentFixture<VehicleReleaseConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReleaseConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReleaseConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
