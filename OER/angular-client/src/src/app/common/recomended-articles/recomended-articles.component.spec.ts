import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecomendedArticlesComponent } from './recomended-articles.component';

describe('RecomendedArticlesComponent', () => {
  let component: RecomendedArticlesComponent;
  let fixture: ComponentFixture<RecomendedArticlesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecomendedArticlesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecomendedArticlesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
