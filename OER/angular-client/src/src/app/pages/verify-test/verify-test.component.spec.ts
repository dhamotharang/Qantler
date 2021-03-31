import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyTestComponent } from './verify-test.component';

describe('VerifyTestComponent', () => {
  let component: VerifyTestComponent;
  let fixture: ComponentFixture<VerifyTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerifyTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifyTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
