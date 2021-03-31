import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiwanIdentityComponent } from './diwan-identity.component';

describe('DiwanIdentityComponent', () => {
  let component: DiwanIdentityComponent;
  let fixture: ComponentFixture<DiwanIdentityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiwanIdentityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiwanIdentityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
