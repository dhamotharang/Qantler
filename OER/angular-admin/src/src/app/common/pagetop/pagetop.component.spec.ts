import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PagetopComponent } from './pagetop.component';

describe('PagetopComponent', () => {
  let component: PagetopComponent;
  let fixture: ComponentFixture<PagetopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PagetopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PagetopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
