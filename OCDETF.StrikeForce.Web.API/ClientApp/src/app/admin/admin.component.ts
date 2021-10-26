import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { LookupService } from '../lookup.service';
import { QuarterlyReport } from '../models/quarterly-report';
import { Strikeforcenames } from '../models/strikeforcenames';
import { User } from '../models/user';
import { SessionService } from '../session.service';
import { TabDirective } from 'ngx-bootstrap/tabs';
import * as moment from 'moment';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  firstTab: boolean = false;
  secondTab: boolean = true;

  currentReport: QuarterlyReport = new QuarterlyReport();

  newUser: User = new User();
  strikeForceSelection: boolean = false;

  currentUser:User = new User();
  allUsers: User[] = [];
  allForceNames: Strikeforcenames[] = [];

  constructor(private userSession: SessionService, private lookupService:LookupService, private toastr: ToastrService) {
    this.currentReport = userSession.currentSession;
    this.currentUser = userSession.getUserSession();
    this.initiateNewUser();
    

    this.getAllUsers();

    this.lookupService.GetForceNames().subscribe(data => {
      this.allForceNames = data;
    });
  }

  getAllUsers() {
    this.userSession.getAllUsers().subscribe(data => {
      this.allUsers = data;
      
    });
  }

  ngOnInit(): void {
  }

  saveUser() {

    if (this.newUser.StrikeForceID.length == 0) {
      this.toastr.error("Please select Strike Force Location!");
      return;
    }
    if (this.newUser.FirstName.trim().length == 0) {
      this.toastr.error("First Name is required!");
      return;
    }
    if (this.newUser.LastName.trim().length == 0) {
      this.toastr.error("Last Name is required!");
      return;      
    }
    if (this.newUser.Email.length == 0) {
      this.toastr.error("Email is required!");
      return;
    }

    if (!this.newUser.Administrator && !this.newUser.Owner && !this.newUser.Contributor) {
      this.toastr.error("Atleast one Role is required!");
      return;
    }

    this.userSession.addNewUser(this.newUser).subscribe(data => {      
      this.getAllUsers();
      if (data.ID.length > 0) {
        this.initiateNewUser();
        this.toastr.success("Successfully Saved!!");
      }        
      else
        this.toastr.error("Unable to Save! User might already exists!");

    });
  }

  
  initiateNewUser() {
    this.newUser.FirstName = "";
    this.newUser.LastName = "";
    this.newUser.Email = "";
    this.newUser.Password = "";
    this.newUser.Owner = true;
    this.newUser.Administrator = false;
    this.newUser.Contributor = true;
    this.newUser.IsActive = true;
    //this.newUser.StrikeForceID = this.currentUser.StrikeForceID;

    this.strikeForceSelection = !this.currentUser.Administrator;
  }

  cancelUser() {}
  
}
