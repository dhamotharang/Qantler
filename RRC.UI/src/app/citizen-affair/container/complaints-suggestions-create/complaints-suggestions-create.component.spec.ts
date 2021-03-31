import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplaintsSuggestionsCreateComponent } from './complaints-suggestions-create.component';

describe('ComplaintsSuggestionsCreateComponent', () => {
  let component: ComplaintsSuggestionsCreateComponent;
  let fixture: ComponentFixture<ComplaintsSuggestionsCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplaintsSuggestionsCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplaintsSuggestionsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
