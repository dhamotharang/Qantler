import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageHeaderTopComponent } from './page-header-top.component';

describe('PageHeaderTopComponent', () => {
  let component: PageHeaderTopComponent;
  let fixture: ComponentFixture<PageHeaderTopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageHeaderTopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageHeaderTopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
