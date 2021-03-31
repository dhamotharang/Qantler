import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplaintSuggestionComponent } from './complaints-suggestions.component';

describe('ComplaintSuggestionComponent', () => {
  let component: ComplaintSuggestionComponent;
  let fixture: ComponentFixture<ComplaintSuggestionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplaintSuggestionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplaintSuggestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
