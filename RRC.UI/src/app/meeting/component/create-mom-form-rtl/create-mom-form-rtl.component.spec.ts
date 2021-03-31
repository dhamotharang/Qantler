import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMomFormRtlComponent } from './create-mom-form-rtl.component';

describe('CreateMomFormRtlComponent', () => {
  let component: CreateMomFormRtlComponent;
  let fixture: ComponentFixture<CreateMomFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateMomFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateMomFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
