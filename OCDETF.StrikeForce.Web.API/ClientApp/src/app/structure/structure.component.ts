import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuarterlyReport } from '../models/quarterly-report';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-structure',
  templateUrl: './structure.component.html',
  styleUrls: ['./structure.component.css']
})
export class StructureComponent implements OnInit {

  currentReport: QuarterlyReport = new QuarterlyReport();
  constructor(private route: ActivatedRoute, private router: Router, private qreportService: QuarterlyreportService, public session: SessionService) {

   
    this.currentReport = session.currentSession;

  }

  ngOnInit(): void {
  }

}
