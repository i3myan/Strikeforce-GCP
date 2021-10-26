import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllocdetfComponent } from './allocdetf.component';

describe('AllocdetfComponent', () => {
  let component: AllocdetfComponent;
  let fixture: ComponentFixture<AllocdetfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllocdetfComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AllocdetfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
