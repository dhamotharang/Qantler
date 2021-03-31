import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaRequestListComponent } from './media-request-list.component';

describe('MediaRequestListComponent', () => {
  let component: MediaRequestListComponent;
  let fixture: ComponentFixture<MediaRequestListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaRequestListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
