import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { QuarterlyReport } from './models/quarterly-report';
import { User } from './models/user';
import { SessionService } from './session.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'App';
  strikeForceName = '';

  showProgress: boolean = false;
  currentUser: User = new User();
  authenticated: Subscription;
  httpProgress: Subscription;



  linkID = 0;
  constructor(
    private route: ActivatedRoute,
    public userSession: SessionService,
    private titleService:Title,
    private router: Router) {
    this.authenticated = userSession.userSessionInitiated$.subscribe(
      session => {
        this.currentUser = session;
        this.titleService.setTitle(this.currentUser.StrikeForceName + " Strike Force Quarterly Report")
        
      });

    this.httpProgress = userSession.showProgressInitiated$.subscribe(progress => {
      this.showProgress = progress;
    });
  }

  logout() {
    this.currentUser = new User();
    this.userSession.logout();
  
    this.router.navigate(['/login']);
  }
  
}
