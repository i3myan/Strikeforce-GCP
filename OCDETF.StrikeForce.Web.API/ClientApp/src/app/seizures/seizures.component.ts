import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppConfigurationService } from '../app-configuration.service';
import { QuarterlyActivity } from '../models/quarterly-activity';
import { QuarterlyReport } from '../models/quarterly-report';
import { User } from '../models/user';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-seizures',
  templateUrl: './seizures.component.html',
  styleUrls: ['./seizures.component.css']
})
export class SeizuresComponent implements OnInit {

  newSeizure: string = "";
  lockQtr: boolean = true;

  unlockQ1: boolean = false;
  unlockQ2: boolean = false;
  unlockQ3: boolean = false;
  unlockQ4: boolean = false;

  currentUser: User = new User();
  
  currentReport: QuarterlyReport = new QuarterlyReport();
  
  constructor(private route: ActivatedRoute, private toastr:ToastrService, private router: Router, private qreportService: QuarterlyreportService, public session: SessionService) {
    this.currentUser = session.getUserSession();
    
    this.currentReport = session.currentSession;
    
    if (this.currentReport.ID.length == 0) {
      let quarterlyReportID = this.router.url.split('/')[2];
      this.qreportService.getReport(quarterlyReportID).subscribe(data => {
        this.session.setCurrentSession(data);
        this.currentReport = session.currentSession;
        this.lock();
        this.getData();
      });
    }
    else {
      this.lock();
      this.getData();
    }
  }

  ngOnInit(): void {
    
  }
  clickQuarter(quarter: string) {
    this.qreportService.getReportByYear(this.currentReport.FiscalYear, this.currentReport.StrikeForceID).subscribe(data => {

      let selectQuarter: QuarterlyReport = data.filter(x => x.Quarter == quarter)[0];
      this.router.navigate(['/redirect/' + selectQuarter.ID + '/seizures']);
    });
  }

  getData() {
    this.qreportService.getSeizure(this.currentReport.ID).subscribe(data => {
      this.currentReport.SeizuresData = data;
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

  onBlur(row: QuarterlyActivity) {

    row.Total = String(Number(row.FirstQuarter == undefined ? 0 : row.FirstQuarter) + Number(row.SecondQuarter == undefined ? 0 : row.SecondQuarter) + Number(row.ThirdQuarter == undefined ? 0 : row.ThirdQuarter) + Number(row.FourthQuarter == undefined ? 0 : row.FourthQuarter));
  }

  addNewSeizure() {
    if (this.newSeizure.trim().length > 0) {
      let newItem: QuarterlyActivity = new QuarterlyActivity();
      newItem.ActivityName = this.newSeizure;
      newItem.QuarterlyReportID = this.currentReport.ID;
      newItem.StrikeForceID = this.currentReport.StrikeForceID;

      newItem.FirstQuarter = "";
      newItem.SecondQuarter = "";
      newItem.ThirdQuarter = "";
      newItem.FourthQuarter = "";
      newItem.Total = undefined;
      debugger;
      newItem.Order = this.currentReport.SeizuresData.length + 1;
      this.qreportService.addNewSeizure(newItem).subscribe(data => {
        this.currentReport.SeizuresData.push(data);
        this.newSeizure = "";
        this.toastr.success("Successfully add Seizure!");
      });
    }
  }
}
