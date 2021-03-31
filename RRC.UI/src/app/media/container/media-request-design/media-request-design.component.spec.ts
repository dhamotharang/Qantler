import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestDesignComponent } from './media-request-design.component';

describe('MediaRequestDesignComponent', () => {
  let component: MediaRequestDesignComponent;
  let fixture: ComponentFixture<MediaRequestDesignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestDesignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestDesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
