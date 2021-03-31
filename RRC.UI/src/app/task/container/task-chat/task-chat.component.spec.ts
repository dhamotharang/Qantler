import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskChatComponent } from './task-chat.component';

describe('TaskChatComponent', () => {
  let component: TaskChatComponent;
  let fixture: ComponentFixture<TaskChatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskChatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
