import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from './models/user';
import { SessionService } from './session.service';



@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authenticationService: SessionService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    
    let currentUser: User = this.authenticationService.getUserSession();
    if (currentUser.IsAuthenticated)
      return true;
    else {
      
      //check if it is available in local storage...in case screen is refreshed.
      if (localStorage.getItem("strikeForceSession") === null) {
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
      else {
        let temp: any = localStorage.getItem('strikeForceSession');
        let currentUser: any = JSON.parse(temp);

        this.authenticationService.setUserSession(currentUser);
        if (this.authenticationService.getUserSession().IsAuthenticated)
          return true;
      }
    }
    
    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
