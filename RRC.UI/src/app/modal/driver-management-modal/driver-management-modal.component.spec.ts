import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverManagementModalComponent } from './driver-management-modal.component';

describe('DriverManagementModalComponent', () => {
  let component: DriverManagementModalComponent;
  let fixture: ComponentFixture<DriverManagementModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverManagementModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverManagementModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
