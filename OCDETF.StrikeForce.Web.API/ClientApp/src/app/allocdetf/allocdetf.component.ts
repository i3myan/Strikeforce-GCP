import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConfigurationService } from '../app-configuration.service';
import { QuarterlyActivity } from '../models/quarterly-activity';
import { QuarterlyReport } from '../models/quarterly-report';
import { User } from '../models/user';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-allocdetf',
  templateUrl: './allocdetf.component.html',
  styleUrls: ['./allocdetf.component.css']
})
export class AllocdetfComponent implements OnInit {

  newActivityName: string = "";
  currentReport: QuarterlyReport = new QuarterlyReport();
  lockQtr: boolean = true;
  currentUser: User = new User();
  unlockQ1: boolean = false;
  unlockQ2: boolean = false;
  unlockQ3: boolean = false;
  unlockQ4: boolean = false;

  

  constructor(private route: ActivatedRoute, private router: Router, private qreportService: QuarterlyreportService, public session: SessionService) {

    this.currentReport = session.currentSession;
    this.currentUser = session.getUserSession();

    if (this.currentReport.ID.length == 0) {
      let quarterlyReportID = this.router.url.split('/')[2];
      this.qreportService.getReport(quarterlyReportID).subscribe(data => {
        this.session.setCurrentSession(data);
        this.currentReport = this.session.currentSession;
        this.getData();
        this.lock();
      });
    }
    else {
      this.getData();
      this.lock();
    }
  }
  

  ngOnInit(): void {
    
  }

  clickQuarter(quarter: string) {
    this.qreportService.getReportByYear(this.currentReport.FiscalYear, this.currentReport.StrikeForceID).subscribe(data => {

      let selectQuarter: QuarterlyReport = data.filter(x => x.Quarter == quarter)[0];
      this.router.navigate(['/redirect/' + selectQuarter.ID + '/allocdetf']);
    });
  }


  lock() {

    if (this.currentReport.Quarter == "4") {
      this.unlockQ4 = false;
      this.unlockQ3 = true;
      this.unlockQ2 = true;
      this.unlockQ1 = true;
    }
    else if (this.currentReport.Quarter == "3") {
      this.unlockQ4 = true;
      this.unlockQ3 = false;
      this.unlockQ2 = true;
      this.unlockQ1 = true;
    }
    else if (this.currentReport.Quarter == "2") {
      this.unlockQ4 = true;
      this.unlockQ3 = true;
      this.unlockQ2 = false;
      this.unlockQ1 = true;
    }
    else if (this.currentReport.Quarter == "1") {
      this.unlockQ4 = true;
      this.unlockQ3 = true;
      this.unlockQ2 = true;
      this.unlockQ1 = false;
    }
  }

  unlock() {
    this.lockQtr = !this.lockQtr;
    if (this.currentUser.Administrator && !this.lockQtr) {
      if (this.currentReport.Quarter == "4") {
        this.unlockQ4 = false;
        this.unlockQ3 = false;
        this.unlockQ2 = false;
        this.unlockQ1 = false;
      }
      else if (this.currentReport.Quarter == "3") {
        this.unlockQ4 = true;
        this.unlockQ3 = false;
        this.unlockQ2 = false;
        this.unlockQ1 = false;
      }
      else if (this.currentReport.Quarter == "2") {
        this.unlockQ4 = true;
        this.unlockQ3 = true;
        this.unlockQ2 = false;
        this.unlockQ1 = false;
      }
      else if (this.currentReport.Quarter == "1") {
        this.unlockQ4 = true;
        this.unlockQ3 = true;
        this.unlockQ2 = true;
        this.unlockQ1 = false;
      }
    }
    else {
      this.lock();
    }
  }


  getData() {
    this.qreportService.getRequired(this.currentReport.ID).subscribe(data => {
      this.currentReport.RequiredData = data;
    });
  }

  onBlur(row: QuarterlyActivity) {
    debugger; 
    row.Total = String(Number(row.FirstQuarter) + Number(row.SecondQuarter) + Number(row.ThirdQuarter) + Number(row.FourthQuarter));   
  }
  
}
