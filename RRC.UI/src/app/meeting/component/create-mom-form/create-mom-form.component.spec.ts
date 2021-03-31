import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMomFormComponent } from './create-mom-form.component';

describe('CreateMomFormComponent', () => {
  let component: CreateMomFormComponent;
  let fixture: ComponentFixture<CreateMomFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateMomFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateMomFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
