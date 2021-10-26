import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../models/user';
import { SessionService } from '../session.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginUser: User = new User();
  

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private toastr:ToastrService,
    private userSession: SessionService) {
    
  }

  ngOnInit(): void {
    if (environment.MicrosoftAD)
      this.loginMicrosoft();
  }

  loginMicrosoft() {
    this.userSession.loginMicrosoftAD().subscribe(data => {
      this.userSession.setUserSession(data);
      if (data.IsAuthenticated) {
        this.router.navigate(['/home']);
      }
    });
  }

  login() {
    
    this.userSession.authenticate(this.loginUser).subscribe(
      data => {
        
        this.userSession.setUserSession(data);
        if (data.IsAuthenticated) {
          this.router.navigate(['/home']);
        }
        else {
          this.toastr.error("Invalid UserName/Password!");          
        }
      });
  }

}
