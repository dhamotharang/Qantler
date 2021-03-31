import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestDesignFormComponent } from './media-request-design-form.component';

describe('MediaRequestDesignFormComponent', () => {
  let component: MediaRequestDesignFormComponent;
  let fixture: ComponentFixture<MediaRequestDesignFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestDesignFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestDesignFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
