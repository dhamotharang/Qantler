import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotographerRequestEditComponent } from './photographer-request-edit.component';

describe('PhotographerRequestEditComponent', () => {
  let component: PhotographerRequestEditComponent;
  let fixture: ComponentFixture<PhotographerRequestEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotographerRequestEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotographerRequestEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
