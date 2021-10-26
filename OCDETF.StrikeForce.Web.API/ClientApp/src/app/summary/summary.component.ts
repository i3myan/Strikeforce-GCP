import { BlockScrollStrategy } from '@angular/cdk/overlay';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { SummaryAnalysis } from '../models/summaryanalysis';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';


@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {

  selectQuarter: string = "1";
  selectFiscalYear: string = Number(moment().format("YYYY")).toString();
  fiscalYears: string[] = [];
  fiscalQuarters: string[] = ["1", "2", "3", "4"];
  blob: Blob = new Blob();

  dataSource: MatTableDataSource<SummaryAnalysis> = new MatTableDataSource<SummaryAnalysis>();
  @ViewChild(MatSort) sort!: MatSort;
  selectSummary: string = "Seizures";
  displayedColumns: string[] = ['Order', 'ActivityName', 'Atlanta', 'Baltimore', 'Boston', 'Chicago', 'Cleveland', 'Dallas', 'Denver', 'Detroit', 'ElPaso', 'Houston', 'KansasCity', 'LosAngeles', 'NewYork', 'Phoenix', 'Sacramento', 'SanDiego', 'SanJuan', 'StLouis', 'Tampa', 'Total'];
  summaryData: SummaryAnalysis[] = [];

  constructor(private qtrService: QuarterlyreportService, private userSession:SessionService) {
    this.getData();
    this.fiscalYears = this.userSession.getYears();
  }

  ngAfterViewInit() {
     
  }

  ngOnInit(): void {
    
  }

  getData() {
    this.qtrService.getSummary(this.selectFiscalYear, this.selectQuarter, this.selectSummary).subscribe(data => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.sort = this.sort;
    });
  }

  createPDF() {
    debugger;
    this.qtrService.createAnalysisPDF(this.selectFiscalYear, this.selectQuarter).subscribe((data) => {
      
      let blob = new Blob([data], { type: "application/pdf" });
      let url = window.URL.createObjectURL(blob);
      let pwa = window.open(url);
      if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
        alert('Please disable your Pop-up blocker and try again.');
      }

    });
  }

  onChange() {
    this.getData();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
