import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkToModalComponent } from './link-to-modal.component';

describe('LinkToModalComponent', () => {
  let component: LinkToModalComponent;
  let fixture: ComponentFixture<LinkToModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinkToModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkToModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
