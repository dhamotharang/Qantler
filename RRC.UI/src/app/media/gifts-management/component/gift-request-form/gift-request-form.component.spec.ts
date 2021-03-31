import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftRequestFormComponent } from './gift-request-form.component';

describe('GiftRequestFormComponent', () => {
  let component: GiftRequestFormComponent;
  let fixture: ComponentFixture<GiftRequestFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftRequestFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftRequestFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
