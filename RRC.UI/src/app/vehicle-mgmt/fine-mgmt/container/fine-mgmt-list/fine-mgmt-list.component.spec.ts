import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FineMgmtListComponent } from './fine-mgmt-list.component';

describe('FineMgmtListComponent', () => {
  let component: FineMgmtListComponent;
  let fixture: ComponentFixture<FineMgmtListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FineMgmtListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FineMgmtListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
