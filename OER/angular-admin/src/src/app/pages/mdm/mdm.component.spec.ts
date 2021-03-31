import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MdmComponent } from './mdm.component';

describe('MdmComponent', () => {
  let component: MdmComponent;
  let fixture: ComponentFixture<MdmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MdmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MdmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
