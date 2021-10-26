import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { QuarterlyReport } from './models/quarterly-report';
import { HttpClient } from '@angular/common/http';
import { QuarterlyActivity } from './models/quarterly-activity';
import { Staffing } from './models/staffing';

@Injectable({
  providedIn: 'root'
})
export class StaffingService {

  constructor(private http: HttpClient) { }

  
}
