import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalDocumentsModalComponent } from './legal-documents-modal.component';

describe('LegalDocumentsModalComponent', () => {
  let component: LegalDocumentsModalComponent;
  let fixture: ComponentFixture<LegalDocumentsModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegalDocumentsModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegalDocumentsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
