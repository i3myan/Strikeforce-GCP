import { Component, Directive, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuarterlyReport } from '../models/quarterly-report';
import { SessionService } from '../session.service';
import { QuarterlyreportService } from '../quarterlyreport.service';

@Component({
  selector: 'app-mission',
  templateUrl: './mission.component.html',
  styleUrls: ['./mission.component.css']
})
export class MissionComponent implements OnInit {

 
  currentReport: QuarterlyReport = new QuarterlyReport();
  constructor(private route: ActivatedRoute,
    private router: Router,
    private qreportService: QuarterlyreportService,
    public session: SessionService) {

    
    this.currentReport = session.currentSession;
  }
  ngOnInit(): void {
  }

}
