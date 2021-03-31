import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrDocumentsModalComponent } from './hr-documents-modal.component';

describe('HrDocumentsModalComponent', () => {
  let component: HrDocumentsModalComponent;
  let fixture: ComponentFixture<HrDocumentsModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrDocumentsModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrDocumentsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
