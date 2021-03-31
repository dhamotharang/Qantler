import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrDocumentsComponent } from './hr-documents.component';

describe('HrDocumentsComponent', () => {
  let component: HrDocumentsComponent;
  let fixture: ComponentFixture<HrDocumentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrDocumentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
