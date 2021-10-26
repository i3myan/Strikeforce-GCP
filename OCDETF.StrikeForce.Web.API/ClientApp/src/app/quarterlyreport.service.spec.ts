import { TestBed } from '@angular/core/testing';

import { QuarterlyreportService } from './quarterlyreport.service';

describe('QuarterlyreportService', () => {
  let service: QuarterlyreportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(QuarterlyreportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
