import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficialTaskComponent } from './official-task.component';

describe('OfficialTaskComponent', () => {
  let component: OfficialTaskComponent;
  let fixture: ComponentFixture<OfficialTaskComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficialTaskComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficialTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
