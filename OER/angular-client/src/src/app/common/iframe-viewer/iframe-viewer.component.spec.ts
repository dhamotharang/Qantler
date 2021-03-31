import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IFrameViewerComponent } from './iframe-viewer.component';

describe('IFrameViewerComponent', () => {
  let component: IFrameViewerComponent;
  let fixture: ComponentFixture<IFrameViewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IFrameViewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IFrameViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
