import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftRequestCreateComponent } from './gift-request-create.component';

describe('GiftRequestCreateComponent', () => {
  let component: GiftRequestCreateComponent;
  let fixture: ComponentFixture<GiftRequestCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GiftRequestCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GiftRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
