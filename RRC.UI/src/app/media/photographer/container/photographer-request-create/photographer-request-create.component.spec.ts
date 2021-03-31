import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotographerRequestCreateComponent } from './photographer-request-create.component';

describe('PhotographerRequestCreateComponent', () => {
  let component: PhotographerRequestCreateComponent;
  let fixture: ComponentFixture<PhotographerRequestCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PhotographerRequestCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PhotographerRequestCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
