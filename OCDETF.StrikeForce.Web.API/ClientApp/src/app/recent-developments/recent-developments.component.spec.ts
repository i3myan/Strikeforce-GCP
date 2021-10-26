import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentDevelopmentsComponent } from './recent-developments.component';

describe('RecentDevelopmentsComponent', () => {
  let component: RecentDevelopmentsComponent;
  let fixture: ComponentFixture<RecentDevelopmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecentDevelopmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecentDevelopmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
