import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { QuarterlyReport } from '../models/quarterly-report';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-printreport',
  templateUrl: './printreport.component.html',
  styleUrls: ['./printreport.component.css']
})
export class PrintreportComponent implements OnInit {

  myReports: QuarterlyReport[] = [];
  selectReportID: string = "";
  currentReport: QuarterlyReport = new QuarterlyReport();
  totalAgents: number =0 ;
  totalAnalysts: number =0;
  totalFederal: number =0;
  totalOther: number =0;
  totalAgency: number =0;

  fromReportID: string = "";
  returnRoute: string = "";

  backLabel: string = "";

  constructor(private session: SessionService, private qtrService: QuarterlyreportService, private router: Router) {

    this.fromReportID = this.router.url.split('/')[2];
    this.returnRoute = this.router.url.split('/')[3]; //View or myReports

    this.qtrService.getReports(this.session.getUserSession().StrikeForceID).subscribe(data => {
      this.myReports = data;
    });

    this.loadReportTables(this.fromReportID);
  }
  
  ngOnInit(): void {
  }

  backTo() {
    if (this.returnRoute == "View") {
      this.router.navigate(['/quarterly', this.fromReportID]);
    }
    else if (this.returnRoute == "myReports") {
      this.router.navigate(['/myreports']);
    }
    else {
      this.router.navigate(['/home']);
    }
  }

  loadReportTables(reportID: string) {
    this.qtrService.getReport(reportID).subscribe(data => {
      this.currentReport = data;

      //Load Staffing
      this.qtrService.getStaffing(reportID).subscribe(data => {
        debugger;
        this.currentReport.StaffingData = data;
        this.calculateTotals();
      });

      //Load Required
      this.qtrService.getRequired(reportID).subscribe(data => {
        this.currentReport.RequiredData = data;
      });

      //Load Specific
      this.qtrService.getSpecific(reportID).subscribe(data => {
        this.currentReport.SpecificData = data;
      });

      //Load OCDETF Only
      this.qtrService.getOcdetfOnly(reportID).subscribe(data => {
        this.currentReport.OCDETFCases = data;
      });

      //Load Seizures
      this.qtrService.getSeizure(reportID).subscribe(data => {
        this.currentReport.SeizuresData = data;
      });
    });
    
  }

  onChange() {
    this.loadReportTables(this.selectReportID);
    
    
    
  }

  calculateTotals() {
    this.totalAgency = 0;
    this.totalAgents = 0;
    this.totalAnalysts = 0;
    this.totalFederal = 0;
    this.totalOther = 0;
    for (let i = 0; i <= this.currentReport.StaffingData.length - 1; i++) {
      this.totalAgents = this.totalAgents + Number(this.currentReport.StaffingData[i].NumberOfAgents);
      this.totalAnalysts = this.totalAnalysts + Number(this.currentReport.StaffingData[i].NumberOfAnalysts);
      this.totalFederal = this.totalFederal + Number(this.currentReport.StaffingData[i].NumberOfFederalTFOs);
      this.totalOther = this.totalOther + Number(this.currentReport.StaffingData[i].OtherNumbers);
      this.totalAgency = this.totalAgency + Number(this.currentReport.StaffingData[i].Total);
    }
  }

  printReport() {
    this.router.navigate(['initiate']);
  }
}
