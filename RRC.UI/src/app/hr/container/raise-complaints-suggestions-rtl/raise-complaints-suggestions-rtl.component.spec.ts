import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RaiseComplaintsSuggestionsRtlComponent } from './raise-complaints-suggestions-rtl.component';

describe('RaiseComplaintsSuggestionsRtlComponent', () => {
  let component: RaiseComplaintsSuggestionsRtlComponent;
  let fixture: ComponentFixture<RaiseComplaintsSuggestionsRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RaiseComplaintsSuggestionsRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RaiseComplaintsSuggestionsRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
