import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppConfigurationService } from '../app-configuration.service';
import { QuarterlyActivity } from '../models/quarterly-activity';
import { QuarterlyReport } from '../models/quarterly-report';
import { Staffing } from '../models/staffing';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';


@Component({
  selector: 'app-staffings',
  templateUrl: './staffings.component.html',
  styleUrls: ['./staffings.component.css']
})
export class StaffingsComponent implements OnInit {
  newActivityName: string = "";
  currentReport: QuarterlyReport = new QuarterlyReport();
  totalAgents: number | undefined;
  totalAnalysts: number | undefined;
  totalFederal: number | undefined;
  totalOther: number | undefined;
  totalAgency: number | undefined;

  row: number = 0;

  constructor(private route: ActivatedRoute, private router: Router, private toastr:ToastrService, private qreportService: QuarterlyreportService, public session: SessionService) {
    
    this.currentReport = session.currentSession;
    

    if (this.currentReport.ID.length == 0) {
      let quarterlyReportID = this.router.url.split('/')[2];
      this.qreportService.getReport(quarterlyReportID).subscribe(data => {
        this.session.setCurrentSession(data);
        this.currentReport = session.currentSession;
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
    this.qreportService.getStaffing(this.currentReport.ID).subscribe(data => {
      this.currentReport.StaffingData = data;
      this.calculateTotals();
    });
  }
  
  onBlur(row: Staffing) {
 
    row.Total = Number(row.NumberOfAgents == undefined ? 0 : row.NumberOfAgents) + Number(row.NumberOfAnalysts == undefined ? 0 : row.NumberOfAnalysts) + Number(row.NumberOfFederalTFOs == undefined ? 0 : row.NumberOfFederalTFOs) + Number(row.OtherNumbers == undefined ? 0 : row.OtherNumbers);

    this.calculateTotals();
    
  }

  calculateTotals() {
    this.totalAgents = 0;
    this.totalAnalysts = 0;
    this.totalFederal = 0;
    this.totalOther = 0;
    this.totalAgency = 0;
    // length - 2 is to exclude totals line item.
    for (let i = 0; i <= this.currentReport.StaffingData.length - 1; i++) {
      this.totalAgents = this.totalAgents + Number(this.currentReport.StaffingData[i].NumberOfAgents);
      this.totalAnalysts = this.totalAnalysts + Number(this.currentReport.StaffingData[i].NumberOfAnalysts);
      this.totalFederal = this.totalFederal + Number(this.currentReport.StaffingData[i].NumberOfFederalTFOs);
      this.totalOther = this.totalOther + Number(this.currentReport.StaffingData[i].OtherNumbers);
      this.totalAgency = this.totalAgency + Number(this.currentReport.StaffingData[i].Total);
    }
    if (this.totalAgents == 0)
      this.totalAgents = undefined;
    if (this.totalAnalysts == 0)
      this.totalAnalysts = undefined;
    if (this.totalFederal == 0)
      this.totalFederal = undefined;
    if (this.totalOther == 0)
      this.totalOther = undefined;
    if (this.totalAgency == 0)
      this.totalAgency = undefined;
  }

  addNew() {
    if (this.newActivityName.trim().length > 0) {
      let newItem: Staffing = new Staffing();
      newItem.QuarterlyReportID = this.currentReport.ID;
      newItem.StrikeForceID = this.currentReport.StrikeForceID;
      newItem.AgencyName = this.newActivityName;
      newItem.IsCommon = false;
      newItem.NumberOfAgents = undefined;
      newItem.NumberOfFederalTFOs = undefined;
      newItem.NumberOfAnalysts = undefined;
      newItem.OtherNumbers = undefined;
      newItem.Total = undefined;
      newItem.Order = this.currentReport.StaffingData.length + 1;
      newItem.IsCommon = false;

      this.qreportService.addStaffingAgency(newItem).subscribe(data => {
        this.currentReport.StaffingData.push(data);
        this.newActivityName = ""

        this.toastr.success("Successfully added!");
      });
    }
    
  }

  deleteItem(delItem: Staffing) {
    this.qreportService.deleteStaffingAgency(delItem).subscribe(data => {
      this.currentReport.StaffingData.forEach(
        (value, index) => {
          if (value.AgencyName == delItem.AgencyName) this.currentReport.StaffingData.splice(index, 1);
        });
      this.toastr.success("Successfully deleted!");
    })
    
    
  }
};

