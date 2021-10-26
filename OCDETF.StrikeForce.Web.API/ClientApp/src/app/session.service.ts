import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { AppConfigurationService } from './app-configuration.service';
import { QuarterlyReport } from './models/quarterly-report';
import { User } from './models/user';
import { environment } from '../environments/environment'
import * as moment from 'moment';


@Injectable({
  providedIn: 'root'
})
export class SessionService {

  private showProgress = new Subject<boolean>();
  private userAuthenticated = new Subject<User>();
  userSessionInitiated$ = this.userAuthenticated.asObservable();
  showProgressInitiated$ = this.showProgress.asObservable();

  public currentSession: QuarterlyReport = new QuarterlyReport();
  public isAuthenticated: boolean = false;
  private currentUser: User = new User();

  constructor(private appConfig: AppConfigurationService, private http: HttpClient) {

  }

  setProgressBar(show: boolean) {
    this.showProgress.next(show);
  }

  setUserSession(aUser: User) {
    if (aUser.IsAuthenticated) {
      let objUserString:string = JSON.stringify(aUser);
      localStorage.setItem("strikeForceSession", objUserString);
      this.currentUser = aUser;
      this.userAuthenticated.next(this.currentUser);
    }
  }
  

  getUserSession() {
    return this.currentUser;
  }

  getYears():string[] {
    let FiscalYears: string[]=[];
    let startYear: number = Number(moment().format("YYYY"));
    for (let i = startYear - 1; i <= startYear + 2; i++) {
      FiscalYears.push(i.toString());
    }
    return FiscalYears;
  }
  
  authenticate(user: User): Observable<User> {

    return this.http.post<User>(environment.apiURL + "/api/users/login", user);
  }

  loginMicrosoftAD(): Observable<User> {

    return this.http.get<User>(environment.apiURL + "/api/AD/User");
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(environment.apiURL + "/api/users/all");
  }

  addNewUser(user: User): Observable<User> {

    return this.http.post<User>(environment.apiURL + "/api/users/add-user", user);
  }

  public setCurrentSession(value: QuarterlyReport) {
    this.currentSession = value;
  }

  public logout() {
    this.currentUser.Session = "";
    this.currentUser.IsAuthenticated = false;
    localStorage.removeItem("strikeForceSession");
  }

  
}
