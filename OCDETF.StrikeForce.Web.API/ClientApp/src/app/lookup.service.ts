import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfigurationService } from './app-configuration.service';

import { environment } from '../environments/environment'
import { Strikeforcenames } from './models/strikeforcenames';

@Injectable({
  providedIn: 'root'
})
export class LookupService {

  constructor(private appConfig: AppConfigurationService, private http: HttpClient) {

  }

  GetForceNames(): Observable<Strikeforcenames[]> {
    return this.http.get<Strikeforcenames[]>(environment.apiURL + "/api/lookup/all-forces");
  }
}
