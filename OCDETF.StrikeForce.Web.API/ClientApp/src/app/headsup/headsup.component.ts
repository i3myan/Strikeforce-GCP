import { Component, OnInit } from '@angular/core';
import { QuarterlyReport } from '../models/quarterly-report';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-headsup',
  templateUrl: './headsup.component.html',
  styleUrls: ['./headsup.component.css']
})
export class HeadsupComponent implements OnInit {

  currentReport: QuarterlyReport = new QuarterlyReport();
  constructor(private userSession: SessionService) {
    this.currentReport = userSession.currentSession;
  }


  ngOnInit(): void {
  }

}
