import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuarterlyreportComponent } from './quarterlyreport.component';

describe('QuarterlyreportComponent', () => {
  let component: QuarterlyreportComponent;
  let fixture: ComponentFixture<QuarterlyreportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuarterlyreportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuarterlyreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
