import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaProtocolRequestsComponent } from './media-protocol-requests.component';

describe('MediaProtocolRequestsComponent', () => {
  let component: MediaProtocolRequestsComponent;
  let fixture: ComponentFixture<MediaProtocolRequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaProtocolRequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaProtocolRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
