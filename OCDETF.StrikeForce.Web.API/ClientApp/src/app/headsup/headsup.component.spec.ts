import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeadsupComponent } from './headsup.component';

describe('HeadsupComponent', () => {
  let component: HeadsupComponent;
  let fixture: ComponentFixture<HeadsupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HeadsupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HeadsupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
