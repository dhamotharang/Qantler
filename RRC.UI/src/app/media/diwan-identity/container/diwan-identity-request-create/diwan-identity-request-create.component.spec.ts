import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiwanIdentityRequestCreateComponent } from './diwan-identity-request-create.component';

describe('DiwanIdentityRequestCreateComponent', () => {
  let component: DiwanIdentityRequestCreateComponent;
  let fixture: ComponentFixture<DiwanIdentityRequestCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiwanIdentityRequestCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiwanIdentityRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
