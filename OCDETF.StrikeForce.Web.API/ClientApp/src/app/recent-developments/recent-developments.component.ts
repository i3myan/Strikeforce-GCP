import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AppConfigurationService } from '../app-configuration.service';
import { QuarterlyReport } from '../models/quarterly-report';
import { RecentDevelopment } from '../models/recent-development';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-recent-developments',
  templateUrl: './recent-developments.component.html',
  styleUrls: ['./recent-developments.component.css']
})
export class RecentDevelopmentsComponent implements OnInit {

  summaryMaxLength: number = 1000;
  currentReport: QuarterlyReport = new QuarterlyReport();
  newRecentCase: RecentDevelopment = new RecentDevelopment();
  deleteRow: RecentDevelopment = new RecentDevelopment();
  modalRef: BsModalRef = new BsModalRef();

  constructor(private modalService: BsModalService, private toastr: ToastrService, private qreportService: QuarterlyreportService, public session: SessionService) {
    
    this.currentReport = session.currentSession;
    this.getAll();
  }

  openModal(row:RecentDevelopment, template: TemplateRef<any>) {
    this.deleteRow = row;
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit(): void {

  }

  validate():boolean {
    if (this.newRecentCase.AgencyName.length > 0 && this.newRecentCase.SponsorAgency.length > 0 &&
      this.newRecentCase.Summary.length > 10 && this.newRecentCase.CaseType.length > 0)
      return true;
    else
      return false;
  }


  addNew() {
    if (this.currentReport.RecentDevelopments.length > 10) {
      this.toastr.error("Cannot exceeed 10 records!");
      return;
    }
    if (this.newRecentCase.ID == "") {
      this.newRecentCase.StrikeForceID = this.currentReport.StrikeForceID;
      this.newRecentCase.QuarterlyReportID = this.currentReport.ID;
      this.newRecentCase.Order = this.currentReport.RecentDevelopments.length + 1;

      this.qreportService.addNewRecentCase(this.newRecentCase).subscribe(data => {

        if (data.length > 0) {
          this.newRecentCase = new RecentDevelopment();
          this.currentReport.RecentDevelopments = data;
          this.toastr.success("Successfully Added New Case Record!");
        }

      });
    }
    else {
      this.qreportService.updateNewRecentCase(this.newRecentCase).subscribe(data => {

        if (data.length > 0) {
          this.newRecentCase = new RecentDevelopment();
          this.currentReport.RecentDevelopments = data;
          this.toastr.success("Successfully Updated !");
        }

      });
    }
  }

  getAll() {
    this.qreportService.getRecentCases(this.currentReport.ID).subscribe(data => {
      this.currentReport.RecentDevelopments = data;
    });
  }

  

  confirmDelete() {
    
      this.qreportService.deleteRecentCase(this.deleteRow).subscribe(data => {
        this.currentReport.RecentDevelopments = data;
        this.modalService.hide();
      });
    
  }

  editItem(row: RecentDevelopment) {
    this.newRecentCase = row;
  }

}
