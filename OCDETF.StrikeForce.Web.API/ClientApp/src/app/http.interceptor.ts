import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { finalize, map, tap } from 'rxjs/operators'
import { SessionService } from './session.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authenticationService: SessionService, private toastr:ToastrService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    let currentUser = this.authenticationService.getUserSession();
    let ok: string;
    const started = Date.now();

    this.authenticationService.setProgressBar(true);

    if (environment.FormsIdentity) {
      request = request.clone({
        setHeaders: {
          Authorization: 'Bearer ' + currentUser.Session
        }
      });
    }    
    return next.handle(request)
      .pipe(
        tap(
          // Succeeds when there is a response; ignore other events
          event => ok = event instanceof HttpResponse ? 'succeeded' : '',
          // Operation failed; error is an HttpErrorResponse
          error => ok = 'failed'
        ),
        // Log when response observable either completes or errors
        finalize(() => {
          this.authenticationService.setProgressBar(false);
          const elapsed = Date.now() - started;
          const msg = `${request.method} "${request.urlWithParams}"
             ${ok} in ${elapsed} ms.`;
          if (ok == 'failed')
            this.toastr.error("Server Error:" + msg);
        })
      );
  }
}
