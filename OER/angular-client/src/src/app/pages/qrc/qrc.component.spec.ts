import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QRCComponent } from './qrc.component';

describe('QRCComponent', () => {
  let component: QRCComponent;
  let fixture: ComponentFixture<QRCComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QRCComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QRCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
