import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CitizenAffairListComponent } from './citizen-affair-list.component';

describe('CitizenAffairListComponent', () => {
  let component: CitizenAffairListComponent;
  let fixture: ComponentFixture<CitizenAffairListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CitizenAffairListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitizenAffairListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
