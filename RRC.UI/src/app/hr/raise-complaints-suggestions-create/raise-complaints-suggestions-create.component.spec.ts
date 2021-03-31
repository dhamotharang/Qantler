import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RaiseComplaintsSuggestionsCreateComponent } from './raise-complaints-suggestions-create.component';

describe('RaiseComplaintsSuggestionsCreateComponent', () => {
  let component: RaiseComplaintsSuggestionsCreateComponent;
  let fixture: ComponentFixture<RaiseComplaintsSuggestionsCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RaiseComplaintsSuggestionsCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RaiseComplaintsSuggestionsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
