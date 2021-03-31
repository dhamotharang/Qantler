import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SensoryCheckComponent } from './sensory-check.component';

describe('SensoryCheckComponent', () => {
  let component: SensoryCheckComponent;
  let fixture: ComponentFixture<SensoryCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SensoryCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SensoryCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
