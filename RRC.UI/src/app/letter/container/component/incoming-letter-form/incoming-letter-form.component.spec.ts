import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingLetterFormComponent } from './incoming-letter-form.component';

describe('IncomingLetterFormComponent', () => {
  let component: IncomingLetterFormComponent;
  let fixture: ComponentFixture<IncomingLetterFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingLetterFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingLetterFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
