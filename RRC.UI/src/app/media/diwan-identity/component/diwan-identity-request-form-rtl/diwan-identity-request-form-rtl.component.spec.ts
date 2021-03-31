import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiwanIdentityRequestFormRtlComponent } from './diwan-identity-request-form-rtl.component';

describe('DiwanIdentityRequestFormRtlComponent', () => {
  let component: DiwanIdentityRequestFormRtlComponent;
  let fixture: ComponentFixture<DiwanIdentityRequestFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiwanIdentityRequestFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiwanIdentityRequestFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
