import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotographerRequestComponent } from './photographer-request.component';

describe('PhotographerRequestComponent', () => {
  let component: PhotographerRequestComponent;
  let fixture: ComponentFixture<PhotographerRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotographerRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotographerRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
