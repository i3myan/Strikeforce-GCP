import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { switchMap } from 'rxjs/operators';
import { QuarterlyReport } from '../models/quarterly-report';
import { ValidationResult } from '../models/validationresult';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';
import { StaffingService } from '../staffing.service';

@Component({
  selector: 'app-quarterlyreport',
  templateUrl: './quarterlyreport.component.html',
  styleUrls: ['./quarterlyreport.component.css']
})
export class QuarterlyreportComponent implements OnInit {

  reportID: string = "";
  title: string = 'App';
  strikeForceName: string = '';
  linkID: number = 0;
  initiateReport: boolean = false;
  myQuarterlyReports: QuarterlyReport[] = [];
  reportTitle: string = "";
  currentReport: QuarterlyReport = new QuarterlyReport();
  validation: ValidationResult[] = [];
  modalRef: BsModalRef = new BsModalRef();

  constructor(private route: ActivatedRoute, private router: Router,
    private qreportService: QuarterlyreportService,
    private staffingService: StaffingService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    public session: SessionService) {

    this.qreportService.getReports(session.getUserSession().StrikeForceID).subscribe(data => {
      debugger;
      this.myQuarterlyReports = data;

      this.reportID = this.router.url.split('/')[2];
      let child: string = this.router.url.split('/')[3];
      if (child == undefined)
        child = 'history';

      this.qreportService.getReport(this.reportID).subscribe(data => {
        this.session.setCurrentSession(data)
        this.currentReport = this.session.currentSession;

        this.linkID = this.getLinkID(child);
        this.router.navigate(['/quarterly/' + this.reportID + '/' + child]);
      });


    });
  }



  getMessage(message: Event) {

  }

  ngOnInit(): void {
  }

  clickLink(i: number) {
    this.linkID = i;
  }

  openModal(template: TemplateRef<any>) {

    this.validate();
    this.modalRef = this.modalService.show(template, { class: 'modal-lg' });
  }

  validate() {
    this.validation = [];
    this.qreportService.validateReport(this.currentReport.ID).subscribe(data => {
      data.forEach(element => this.validation.push(element));
      this.qreportService.validateStaffing(this.currentReport.ID).subscribe(data2 => {
        data2.forEach(element => this.validation.push(element));
        this.qreportService.validateActivity(this.currentReport.ID).subscribe(data3 => {
          data3.forEach(element => this.validation.push(element));
          
        });
      });
    });
  }

  printPreview() {
    this.router.navigate(['print', this.currentReport.ID, "View"]);
  }

  saveReport() {
   
    this.qreportService.updateReport(this.currentReport).subscribe(dataRpt => {
      this.toastr.success("Successfully Saved!!");
    });

    if (this.currentReport.StaffingData != null && this.currentReport.StaffingData.length > 0) {
      this.qreportService.UpdateStaff(this.currentReport.StaffingData).subscribe(dataStaff => {

      });
    }

    if (this.currentReport.RequiredData != null && this.currentReport.RequiredData.length > 0) {
      this.qreportService.updateMeasures(this.currentReport.RequiredData).subscribe(required => {

      });
    }

    if (this.currentReport.SeizuresData != null && this.currentReport.SeizuresData.length > 0) {
      this.qreportService.updateMeasures(this.currentReport.SeizuresData).subscribe(dataSeizure => {

      });
    }

    if (this.currentReport.SpecificData != null && this.currentReport.SpecificData.length > 0) {
      this.qreportService.updateMeasures(this.currentReport.SpecificData).subscribe(dataSpecific => {
        
      });
    }        
  }

  submitReport() {
    this.currentReport.Status = "Submitted";
    this.qreportService.updateReport(this.currentReport).subscribe(data => {
      this.toastr.success("Successfully Saved!!");
    });
  }

  printReport() {
    this.session.currentSession = this.currentReport;
    this.router.navigate(['print']);
  }

  getLinkID(child: string): number {
    switch (child) {
      case 'history': return 11;
      case 'mission': return 12;
      case 'structure': return 13;
      case 'locations': return 14;
      case 'staffings': return 15;
      case 'allocdetf': return 16;
      case 'ocdetfonly': return 17;
      case 'seizures': return 18;
      case 'specific': return 19;
      case 'challenges': return 20;
      case 'recent': return 21;
      case 'headsup': return 22;
    }
    return 11;
  }

  nextStep() {
    switch (this.linkID) {
      case 11: this.router.navigate(['/quarterly/' + this.reportID + '/mission']); break;
      case 12: this.router.navigate(['/quarterly/' + this.reportID + '/structure']); break;
      case 13: this.router.navigate(['/quarterly/' + this.reportID + '/locations']); break;
      case 14: this.router.navigate(['/quarterly/' + this.reportID + '/staffings']); break;
      case 15: this.router.navigate(['/quarterly/' + this.reportID + '/allocdetf']); break;
      case 16: this.router.navigate(['/quarterly/' + this.reportID + '/ocdetfonly']); break;
      case 17: this.router.navigate(['/quarterly/' + this.reportID + '/seizures']); break;
      case 18: this.router.navigate(['/quarterly/' + this.reportID + '/specific']); break;
      case 19: this.router.navigate(['/quarterly/' + this.reportID + '/challenges']); break;
      case 20: this.router.navigate(['/quarterly/' + this.reportID + '/recent']); break;
      case 21: this.router.navigate(['/quarterly/' + this.reportID + '/headsup']); break;
    }
    if (this.linkID >= 11 && this.linkID <= 21)
      this.linkID++;
  }

  previousStep() {

    switch (this.linkID) {
      case 12: this.router.navigate(['/quarterly/' + this.reportID + '/history']); break;
      case 13: this.router.navigate(['/quarterly/' + this.reportID + '/mission']); break;
      case 14: this.router.navigate(['/quarterly/' + this.reportID + '/structure']); break;
      case 15: this.router.navigate(['/quarterly/' + this.reportID + '/locations']); break;
      case 16: this.router.navigate(['/quarterly/' + this.reportID + '/staffings']); break;
      case 17: this.router.navigate(['/quarterly/' + this.reportID + '/allocdetf']); break;
      case 18: this.router.navigate(['/quarterly/' + this.reportID + '/ocdetfonly']); break;
      case 19: this.router.navigate(['/quarterly/' + this.reportID + '/seizures']); break;
      case 20: this.router.navigate(['/quarterly/' + this.reportID + '/specific']); break;
      case 21: this.router.navigate(['/quarterly/' + this.reportID + '/challenges']); break;
      case 22: this.router.navigate(['/quarterly/' + this.reportID + '/recent']); break;
      case 23: this.router.navigate(['/quarterly/' + this.reportID + '/headsup']); break;
    }
    if (this.linkID >= 2)
      this.linkID--;
  }

}
