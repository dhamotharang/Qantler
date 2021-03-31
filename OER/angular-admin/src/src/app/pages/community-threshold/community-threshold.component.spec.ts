import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityThresholdComponent } from './community-threshold.component';

describe('CommunityThresholdComponent', () => {
  let component: CommunityThresholdComponent;
  let fixture: ComponentFixture<CommunityThresholdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunityThresholdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityThresholdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
