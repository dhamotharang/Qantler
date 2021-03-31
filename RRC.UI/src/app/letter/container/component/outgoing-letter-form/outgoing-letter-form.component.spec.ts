import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OutgoingLetterFormComponent } from './outgoing-letter-form.component';

describe('OutgoingLetterFormComponent', () => {
  let component: OutgoingLetterFormComponent;
  let fixture: ComponentFixture<OutgoingLetterFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutgoingLetterFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutgoingLetterFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
