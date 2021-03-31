import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectionListComponent } from './rejection-list.component';

describe('RejectionListComponent', () => {
  let component: RejectionListComponent;
  let fixture: ComponentFixture<RejectionListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RejectionListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RejectionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
