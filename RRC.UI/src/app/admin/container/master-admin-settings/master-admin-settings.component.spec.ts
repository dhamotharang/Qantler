import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterAdminSettingsComponent } from './master-admin-settings.component';

describe('MasterAdminSettingsComponent', () => {
  let component: MasterAdminSettingsComponent;
  let fixture: ComponentFixture<MasterAdminSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterAdminSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterAdminSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
