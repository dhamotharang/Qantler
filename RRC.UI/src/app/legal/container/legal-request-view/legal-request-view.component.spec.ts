import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalRequestViewComponent } from './legal-request-view.component';

describe('LegalRequestViewComponent', () => {
  let component: LegalRequestViewComponent;
  let fixture: ComponentFixture<LegalRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegalRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegalRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
