import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftsManagementHomeComponent } from './gifts-management-home.component';

describe('GiftsManagementHomeComponent', () => {
  let component: GiftsManagementHomeComponent;
  let fixture: ComponentFixture<GiftsManagementHomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftsManagementHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftsManagementHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
