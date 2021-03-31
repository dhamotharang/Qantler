import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenAffairCreateComponent } from './citizen-affair-create.component';

describe('CitizenAffairCreateComponent', () => {
  let component: CitizenAffairCreateComponent;
  let fixture: ComponentFixture<CitizenAffairCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CitizenAffairCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenAffairCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
