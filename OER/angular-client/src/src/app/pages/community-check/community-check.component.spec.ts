import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityCheckComponent } from './community-check.component';

describe('CommunityCheckComponent', () => {
  let component: CommunityCheckComponent;
  let fixture: ComponentFixture<CommunityCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunityCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
