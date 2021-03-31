import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverMgmtRouterComponent } from './driver-mgmt-router.component';

describe('DriverMgmtRouterComponent', () => {
  let component: DriverMgmtRouterComponent;
  let fixture: ComponentFixture<DriverMgmtRouterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverMgmtRouterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverMgmtRouterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
