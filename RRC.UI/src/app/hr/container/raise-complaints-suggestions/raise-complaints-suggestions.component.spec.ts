import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RaiseComplaintsSuggestionsComponent } from './raise-complaints-suggestions.component';

describe('RaiseComplaintsSuggestionsComponent', () => {
  let component: RaiseComplaintsSuggestionsComponent;
  let fixture: ComponentFixture<RaiseComplaintsSuggestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RaiseComplaintsSuggestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RaiseComplaintsSuggestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
