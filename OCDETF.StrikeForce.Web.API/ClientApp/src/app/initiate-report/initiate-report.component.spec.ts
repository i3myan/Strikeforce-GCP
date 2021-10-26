import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiateReportComponent } from './initiate-report.component';

describe('InitiateReportComponent', () => {
  let component: InitiateReportComponent;
  let fixture: ComponentFixture<InitiateReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InitiateReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiateReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
