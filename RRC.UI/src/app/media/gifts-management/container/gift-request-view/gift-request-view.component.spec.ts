import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftRequestViewComponent } from './gift-request-view.component';

describe('GiftRequestViewComponent', () => {
  let component: GiftRequestViewComponent;
  let fixture: ComponentFixture<GiftRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
