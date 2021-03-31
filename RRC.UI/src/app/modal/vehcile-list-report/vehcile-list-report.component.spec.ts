import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehcileListReportComponent } from './vehcile-list-report.component';

describe('VehcileListReportComponent', () => {
  let component: VehcileListReportComponent;
  let fixture: ComponentFixture<VehcileListReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehcileListReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehcileListReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
