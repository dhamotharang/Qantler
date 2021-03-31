import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WCMManagementComponent } from './wcmmanagement.component';

describe('WCMManagementComponent', () => {
  let component: WCMManagementComponent;
  let fixture: ComponentFixture<WCMManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WCMManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WCMManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
