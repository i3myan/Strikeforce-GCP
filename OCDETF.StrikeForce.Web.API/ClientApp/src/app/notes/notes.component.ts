import { Component, OnInit } from '@angular/core';
import { QuarterlyReport } from '../models/quarterly-report';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {

  currentReport: QuarterlyReport = new QuarterlyReport();
  constructor(private userSession: SessionService) {
    this.currentReport = userSession.currentSession;
  }

  ngOnInit(): void {

  }

}
