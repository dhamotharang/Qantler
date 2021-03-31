import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficialTaskFormComponent } from './official-task-form.component';

describe('OfficialTaskFormComponent', () => {
  let component: OfficialTaskFormComponent;
  let fixture: ComponentFixture<OfficialTaskFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficialTaskFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficialTaskFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
