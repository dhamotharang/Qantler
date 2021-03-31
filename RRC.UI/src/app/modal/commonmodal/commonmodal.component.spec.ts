import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonModalComponent } from './commonmodal.component';

describe('CommonModalComponent', () => {
  let component: CommonModalComponent;
  let fixture: ComponentFixture<CommonModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommonModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommonModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
