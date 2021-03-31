import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LetterListRtlComponent } from './letter-list-rtl.component';

describe('LetterListRtlComponent', () => {
  let component: LetterListRtlComponent;
  let fixture: ComponentFixture<LetterListRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LetterListRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LetterListRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
