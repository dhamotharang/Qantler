import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardSideMenuComponent } from './dashboard-side-menu.component';

describe('DashboardSideMenuComponent', () => {
  let component: DashboardSideMenuComponent;
  let fixture: ComponentFixture<DashboardSideMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardSideMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardSideMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
