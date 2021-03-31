import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogFineComponent } from './log-fine.component';

describe('LogFineComponent', () => {
  let component: LogFineComponent;
  let fixture: ComponentFixture<LogFineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogFineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogFineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
