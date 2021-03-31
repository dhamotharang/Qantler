import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FineManagementComponent } from './fine-management.component';

describe('FineManagementComponent', () => {
  let component: FineManagementComponent;
  let fixture: ComponentFixture<FineManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FineManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FineManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
