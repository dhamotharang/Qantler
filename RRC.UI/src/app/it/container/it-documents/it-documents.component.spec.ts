import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItDocumentsComponent } from './it-documents.component';

describe('ItDocumentsComponent', () => {
  let component: ItDocumentsComponent;
  let fixture: ComponentFixture<ItDocumentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItDocumentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
