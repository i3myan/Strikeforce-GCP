import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OcdetfonlyComponent } from './ocdetfonly.component';

describe('OcdetfonlyComponent', () => {
  let component: OcdetfonlyComponent;
  let fixture: ComponentFixture<OcdetfonlyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OcdetfonlyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OcdetfonlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
