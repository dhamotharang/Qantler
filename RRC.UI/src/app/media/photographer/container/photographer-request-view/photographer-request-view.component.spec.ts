import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotographerRequestViewComponent } from './photographer-request-view.component';

describe('PhotographerRequestViewComponent', () => {
  let component: PhotographerRequestViewComponent;
  let fixture: ComponentFixture<PhotographerRequestViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotographerRequestViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotographerRequestViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
