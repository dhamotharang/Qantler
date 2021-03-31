import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalRequestFormComponent } from './legal-request-form.component';

describe('LegalRequestFormComponent', () => {
  let component: LegalRequestFormComponent;
  let fixture: ComponentFixture<LegalRequestFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegalRequestFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegalRequestFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
