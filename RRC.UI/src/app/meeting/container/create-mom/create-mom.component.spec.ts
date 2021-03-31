import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMomComponent } from './create-mom.component';

describe('CreateMomComponent', () => {
  let component: CreateMomComponent;
  let fixture: ComponentFixture<CreateMomComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateMomComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateMomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
