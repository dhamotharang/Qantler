import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItDocumentsModalComponent } from './it-documents-modal.component';

describe('ItDocumentsModalComponent', () => {
  let component: ItDocumentsModalComponent;
  let fixture: ComponentFixture<ItDocumentsModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItDocumentsModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItDocumentsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
