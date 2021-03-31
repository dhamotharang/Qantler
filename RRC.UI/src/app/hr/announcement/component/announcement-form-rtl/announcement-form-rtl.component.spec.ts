import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnouncementFormRtlComponent } from './announcement-form-rtl.component';

describe('AnnouncementFormRtlComponent', () => {
  let component: AnnouncementFormRtlComponent;
  let fixture: ComponentFixture<AnnouncementFormRtlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AnnouncementFormRtlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnouncementFormRtlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
