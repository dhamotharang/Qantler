import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplaintsSuggestionsRtlComponent } from './complaints-suggestions-rtl.component';

describe('ComplaintsSuggestionsRtlComponent', () => {
  let component: ComplaintsSuggestionsRtlComponent;
  let fixture: ComponentFixture<ComplaintsSuggestionsRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplaintsSuggestionsRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplaintsSuggestionsRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
