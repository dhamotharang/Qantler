import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminSettingsRtlComponent } from './admin-settings-rtl.component';

describe('AdminSettingsRtlComponent', () => {
  let component: AdminSettingsRtlComponent;
  let fixture: ComponentFixture<AdminSettingsRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminSettingsRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminSettingsRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
