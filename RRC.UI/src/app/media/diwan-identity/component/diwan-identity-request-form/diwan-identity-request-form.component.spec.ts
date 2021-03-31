import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiwanIdentityRequestFormComponent } from './diwan-identity-request-form.component';

describe('DiwanIdentityRequestFormComponent', () => {
  let component: DiwanIdentityRequestFormComponent;
  let fixture: ComponentFixture<DiwanIdentityRequestFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiwanIdentityRequestFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiwanIdentityRequestFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
