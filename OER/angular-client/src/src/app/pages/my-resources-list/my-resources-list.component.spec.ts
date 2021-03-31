import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyResourcesListComponent } from './my-resources-list.component';

describe('MyResourcesListComponent', () => {
  let component: MyResourcesListComponent;
  let fixture: ComponentFixture<MyResourcesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyResourcesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyResourcesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
