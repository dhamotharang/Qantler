import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalRequestCreateComponent } from './legal-request-create.component';

describe('LegalRequestCreateComponent', () => {
  let component: LegalRequestCreateComponent;
  let fixture: ComponentFixture<LegalRequestCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegalRequestCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegalRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
