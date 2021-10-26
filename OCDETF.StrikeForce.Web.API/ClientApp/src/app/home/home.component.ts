import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { QuarterlyReport } from '../models/quarterly-report';
import { User } from '../models/user';
import { QuarterlyreportService } from '../quarterlyreport.service';
import { SessionService } from '../session.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  myQuarterlyReports: QuarterlyReport[] = [];
  currentUser: User = new User();

  constructor(
    private quarterRptService: QuarterlyreportService,
    private router: Router,
    private userSession: SessionService) {
    this.currentUser = userSession.getUserSession();

    this.quarterRptService.getRecentReports(userSession.getUserSession().StrikeForceID).subscribe(data => {
      this.myQuarterlyReports = data;
    });
  }

  ngOnInit(): void {
  }

  getMyQuarterlyReports() {
    
  }

  viewReport(selectRow: QuarterlyReport) {
    this.userSession.setCurrentSession(selectRow);
    //this.messageToEmit.emit(selectRow.StrikeForceName + "( " + selectRow.FiscalYear + " - " + selectRow.Quarter + ")");
    this.router.navigate(['/quarterly', selectRow.ID]);
  }

  getUser() {

  }
}
