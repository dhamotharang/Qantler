import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WCMComponent } from './wcm.component';

describe('WCMComponent', () => {
  let component: WCMComponent;
  let fixture: ComponentFixture<WCMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WCMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WCMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
