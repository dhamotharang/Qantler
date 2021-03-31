import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MakeProfileComponent } from './make-profile.component';

describe('MakeProfileComponent', () => {
  let component: MakeProfileComponent;
  let fixture: ComponentFixture<MakeProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MakeProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MakeProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
