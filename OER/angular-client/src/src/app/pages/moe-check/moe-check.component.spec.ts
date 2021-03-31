import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MoeCheckComponent } from './moe-check.component';

describe('MoeCheckComponent', () => {
  let component: MoeCheckComponent;
  let fixture: ComponentFixture<MoeCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MoeCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MoeCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
