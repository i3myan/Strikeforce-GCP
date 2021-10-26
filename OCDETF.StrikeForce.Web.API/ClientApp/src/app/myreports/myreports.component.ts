import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { QuarterlyReport } from '../models/quarterly-report';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';
import { ToastrService } from 'ngx-toastr';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-myreports',
  templateUrl: './myreports.component.html',
  styleUrls: ['./myreports.component.css']
})
export class MyreportsComponent implements OnInit {

  showNewReportForm: boolean = false;
  newQuarterlyReport: QuarterlyReport = new QuarterlyReport();
  myQuarterlyReports: QuarterlyReport[] = [];
  dataSource: MatTableDataSource<QuarterlyReport> = new MatTableDataSource<QuarterlyReport>();
  displayedColumns: string[] = ['Name', 'StrikeForceName', 'FiscalYear', 'Quarter', 'Status', 'IsActive', 'Delete'];

  @Output() messageToEmit = new EventEmitter<string>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private quarterRptService: QuarterlyreportService,
    private router: Router,
    private toastr: ToastrService,
    private userSession: SessionService)
  {
    
    this.quarterRptService.getReports(userSession.getUserSession().StrikeForceID).subscribe(data => {
      this.myQuarterlyReports = data;
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  ngOnInit(): void {
  }

  createNewReport() {
    this.router.navigate(['initiate']);
    this.showNewReportForm = true;
  }

  

  deleteReport(selectRow:QuarterlyReport) {
    this.quarterRptService.deleteReport(selectRow).subscribe(sel => {
      this.toastr.success("Successfully deleted!");
    });

  }
  closeNewReport() {
    this.showNewReportForm = false;
  }

  viewReport(selectRow: QuarterlyReport) {
    this.userSession.setCurrentSession(selectRow);
    this.messageToEmit.emit(selectRow.StrikeForceName + "( " + selectRow.FiscalYear + " - " + selectRow.Quarter + ")");
    this.router.navigate(['/quarterly', selectRow.ID]);
  }

  printReport(row: QuarterlyReport) {
    this.userSession.currentSession = row;
    this.router.navigate(['print', row.ID, "myReports"]);
  }

  clearForm() {
    this.newQuarterlyReport.FiscalYear = "";
    this.newQuarterlyReport.Quarter = "";
    this.newQuarterlyReport.StrikeForceName = "";

  }
}
