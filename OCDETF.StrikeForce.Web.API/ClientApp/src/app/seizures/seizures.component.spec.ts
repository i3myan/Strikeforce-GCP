import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeizuresComponent } from './seizures.component';

describe('SeizuresComponent', () => {
  let component: SeizuresComponent;
  let fixture: ComponentFixture<SeizuresComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SeizuresComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SeizuresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
