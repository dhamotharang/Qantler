import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProtocolHomepageComponent } from './protocol-homepage.component';

describe('ProtocolHomepageComponent', () => {
  let component: ProtocolHomepageComponent;
  let fixture: ComponentFixture<ProtocolHomepageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProtocolHomepageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProtocolHomepageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
