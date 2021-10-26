import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { LookupService } from '../lookup.service';
import { QuarterlyReport } from '../models/quarterly-report';
import { Strikeforcenames } from '../models/strikeforcenames';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-initiate-report',
  templateUrl: './initiate-report.component.html',
  styleUrls: ['./initiate-report.component.css']
})
export class InitiateReportComponent implements OnInit {

  newQuarterlyReport: QuarterlyReport = new QuarterlyReport();
  
  allForceNames: Strikeforcenames[] = [];
  FiscalYears: string[] = [];

  constructor(private userSession: SessionService,
    private router: Router,
    private quarterRptService: QuarterlyreportService, lookupService: LookupService, private toastr: ToastrService) {
        
    this.startNewReport();
    
    lookupService.GetForceNames().subscribe(data => {
      this.allForceNames = data;
    });
  }

  ngOnInit(): void {

  }



  createNewReport() {


  }

  saveNewReport() {

    if (this.newQuarterlyReport.Name.length == 0)
      this.toastr.error("Report Name Required!");

    if (this.newQuarterlyReport.Name.length > 0) {
      this.quarterRptService.createNewReport(this.newQuarterlyReport).subscribe(
        data => {
          this.startNewReport();
          this.toastr.success("Successfully Saved!");
          this.myReports();
        }
      );
    }
    
  }

  myReports() {
    this.router.navigate(['myreports']);
  }

  startNewReport() {
    this.newQuarterlyReport = new QuarterlyReport();
    this.newQuarterlyReport.StrikeForceID = this.userSession.getUserSession().StrikeForceID;
    let startYear: number = Number(moment().format("YYYY"));
    for (let i = startYear - 1; i <= startYear + 2; i++) {
      this.FiscalYears.push(i.toString());
    }
    this.newQuarterlyReport.FiscalYear = moment().format("YYYY").toString();
    this.newQuarterlyReport.Quarter = moment().format("Q").toString();
  }
}
