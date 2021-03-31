import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverMgmtContainerComponent } from './driver-mgmt-container.component';

describe('DriverMgmtContainerComponent', () => {
  let component: DriverMgmtContainerComponent;
  let fixture: ComponentFixture<DriverMgmtContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverMgmtContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverMgmtContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
