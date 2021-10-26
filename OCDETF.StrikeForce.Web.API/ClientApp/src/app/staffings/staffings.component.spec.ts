import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffingsComponent } from './staffings.component';

describe('StaffingsComponent', () => {
  let component: StaffingsComponent;
  let fixture: ComponentFixture<StaffingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
