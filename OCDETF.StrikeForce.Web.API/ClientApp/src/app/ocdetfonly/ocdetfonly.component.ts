import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { AppConfigurationService } from '../app-configuration.service';
import { QuarterlyActivity } from '../models/quarterly-activity';
import { QuarterlyReport } from '../models/quarterly-report';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-ocdetfonly',
  templateUrl: './ocdetfonly.component.html',
  styleUrls: ['./ocdetfonly.component.css']
})
export class OcdetfonlyComponent implements OnInit {

  currentReport: QuarterlyReport = new QuarterlyReport();
  newMeasure: string = "";

  constructor(private route: ActivatedRoute, private router: Router, private qreportService: QuarterlyreportService, public session: SessionService) {
    this.currentReport = session.currentSession;
    if (this.currentReport.ID.length == 0) {
      let quarterlyReportID = this.router.url.split('/')[2];
      this.qreportService.getReport(quarterlyReportID).subscribe(data => {
        this.session.setCurrentSession(data);
        this.currentReport = this.session.currentSession;
        this.getData();
      });
    }
    else {
      this.getData();
    }
    
    
  }

  ngOnInit(): void {
  }

  getData() {
    this.qreportService.getOcdetfOnly(this.currentReport.ID).subscribe(data => {
      this.currentReport.OCDETFCases = data;
    });
  }

  onBlur(row: QuarterlyActivity) {
    row.Total = String(Number(row.FirstQuarter) + Number(row.SecondQuarter) + Number(row.ThirdQuarter) + Number(row.FourthQuarter));    
  }

  
}
