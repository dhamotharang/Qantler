import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QrcComponent } from './qrc.component';

describe('QrcComponent', () => {
  let component: QrcComponent;
  let fixture: ComponentFixture<QrcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QrcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QrcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
