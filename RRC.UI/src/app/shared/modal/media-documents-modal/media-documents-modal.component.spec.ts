import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaDocumentsModalComponent } from './media-documents-modal.component';

describe('MediaDocumentsModalComponent', () => {
  let component: MediaDocumentsModalComponent;
  let fixture: ComponentFixture<MediaDocumentsModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaDocumentsModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaDocumentsModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
