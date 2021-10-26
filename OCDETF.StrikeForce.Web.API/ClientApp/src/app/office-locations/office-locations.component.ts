import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuarterlyReport } from '../models/quarterly-report';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-office-locations',
  templateUrl: './office-locations.component.html',
  styleUrls: ['./office-locations.component.css']
})
export class OfficeLocationsComponent implements OnInit {

  currentReport: QuarterlyReport = new QuarterlyReport();
  constructor(private route: ActivatedRoute, private router: Router, private qreportService: QuarterlyreportService, public session: SessionService) {
    this.currentReport = session.currentSession;
  }


  ngOnInit(): void {
  }

}
