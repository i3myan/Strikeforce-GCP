import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppConfigurationService } from '../app-configuration.service';
import { QuarterlyActivity } from '../models/quarterly-activity';
import { QuarterlyReport } from '../models/quarterly-report';
import { User } from '../models/user';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-specific',
  templateUrl: './specific.component.html',
  styleUrls: ['./specific.component.css']
})
export class SpecificComponent implements OnInit {

  newMeasure: string = "";
  currentReport: QuarterlyReport = new QuarterlyReport();
  lockQtr: boolean = true;

  unlockQ1: boolean = false;
  unlockQ2: boolean = false;
  unlockQ3: boolean = false;
  unlockQ4: boolean = false;

  Q1: QuarterlyReport = new QuarterlyReport();
  Q2: QuarterlyReport = new QuarterlyReport();
  Q3: QuarterlyReport = new QuarterlyReport();
  Q4: QuarterlyReport = new QuarterlyReport();

  currentUser: User = new User();
  @Output() messageToEmit = new EventEmitter<string>();

  constructor(private route: ActivatedRoute,
    private router: Router,
    private qreportService: QuarterlyreportService,
    private toastr:ToastrService,
    public session: SessionService) {

    this.currentReport = session.currentSession;
    this.currentUser = this.session.getUserSession();
    

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

  viewReport(selectRow: QuarterlyReport) {
    this.session.setCurrentSession(selectRow);
    this.messageToEmit.emit(selectRow.StrikeForceName + "( " + selectRow.FiscalYear + " - " + selectRow.Quarter + ")");
    this.router.navigate(['/quarterly', selectRow.ID]);
  }

  clickQuarter(quarter:string) {
    this.qreportService.getReportByYear(this.currentReport.FiscalYear, this.currentReport.StrikeForceID).subscribe(data => {

      let selectQuarter:QuarterlyReport = data.filter(x => x.Quarter == quarter)[0];
      this.router.navigate(['/redirect/' + selectQuarter.ID + '/specific']);
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


  ngOnInit(): void {
    
  }

  getData() {
    this.qreportService.getSpecific(this.currentReport.ID).subscribe(data => {
      this.currentReport.SpecificData = data;
    });

    
    this.qreportService.getReportByYear(this.currentReport.FiscalYear, this.currentReport.StrikeForceID).subscribe(data => {

      this.Q1 = data.filter(x => x.Quarter == "1")[0];
      this.Q2 = data.filter(x => x.Quarter == "2")[0];
      this.Q3 = data.filter(x => x.Quarter == "3")[0];
      this.Q4 = data.filter(x => x.Quarter == "4")[0];
      debugger;
    });
  }

  onSpecBlur(row: QuarterlyActivity) {
    row.Total = String(Number(row.FirstQuarter) + Number(row.SecondQuarter) + Number(row.ThirdQuarter) + Number(row.FourthQuarter));
  }

  addNewMeasure() {
    if (this.newMeasure.trim().length > 0) {
      let newItem: QuarterlyActivity = new QuarterlyActivity();
      newItem.ActivityName = this.newMeasure;
      newItem.QuarterlyReportID = this.currentReport.ID;
      newItem.StrikeForceID = this.currentReport.StrikeForceID;

      newItem.FirstQuarter = "";
      newItem.SecondQuarter = "";
      newItem.ThirdQuarter = "";
      newItem.FourthQuarter = "";
      newItem.Total = undefined;
      newItem.Order = this.currentReport.SpecificData.length + 1;
      this.qreportService.addSpecificMeasure(newItem).subscribe(data => {
        this.currentReport.SpecificData.push(data);
        this.newMeasure = "";
        this.toastr.success("Successfully add measure!");
      });          
    }

  }

  deleteItem(delItem: QuarterlyActivity) {
    this.qreportService.deleteSpecificMeasure(delItem).subscribe(data => {
      this.currentReport.SpecificData.forEach(
        (value, index) => {
          if (value.ActivityName == delItem.ActivityName)
            this.currentReport.SpecificData.splice(index, 1);
        });

      this.toastr.success("Successfully deleted!");
    });
    

  }
}
