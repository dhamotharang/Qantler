import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehicleReleaseConfirmationContainerComponent } from './vehicle-release-confirmation-container.component';

describe('VehicleReleaseConfirmationContainerComponent', () => {
  let component: VehicleReleaseConfirmationContainerComponent;
  let fixture: ComponentFixture<VehicleReleaseConfirmationContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehicleReleaseConfirmationContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehicleReleaseConfirmationContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
