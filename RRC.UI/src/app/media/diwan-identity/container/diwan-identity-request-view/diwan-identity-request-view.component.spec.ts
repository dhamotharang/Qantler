import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiwanIdentityRequestViewComponent } from './diwan-identity-request-view.component';

describe('DiwanIdentityRequestViewComponent', () => {
  let component: DiwanIdentityRequestViewComponent;
  let fixture: ComponentFixture<DiwanIdentityRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiwanIdentityRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiwanIdentityRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
