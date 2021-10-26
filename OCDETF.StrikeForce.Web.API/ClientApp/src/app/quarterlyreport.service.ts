import { Injectable } from '@angular/core';
import { AppConfigurationService } from './app-configuration.service';
import { QuarterlyReport } from './models/quarterly-report';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { environment } from '../environments/environment'
import { Observable } from 'rxjs';
import { Staffing } from './models/staffing';
import { QuarterlyActivity } from './models/quarterly-activity';
import { RecentDevelopment } from './models/recent-development';
import { SummaryAnalysis } from './models/summaryanalysis';
import { ValidationResult } from './models/validationresult';

@Injectable({
  providedIn: 'root'
})
export class QuarterlyreportService {

  newQuarterlyReport: QuarterlyReport = new QuarterlyReport();
  newBlankReport: QuarterlyReport = new QuarterlyReport();

  constructor(private appConfig: AppConfigurationService, private http: HttpClient) {
    
  }

  

  createNewReport(newReport: QuarterlyReport): Observable<QuarterlyReport> {
    
    return this.http.post<QuarterlyReport>(environment.apiURL + "/api/quarterly/create-new", newReport);
  }

  updateReport(newReport: QuarterlyReport): Observable<QuarterlyReport> {

    return this.http.post<QuarterlyReport>(environment.apiURL + "/api/quarterly/update-report", newReport);
  }

  //Returns refreshed list of reports after soft deletes
  deleteReport(qReport:QuarterlyReport): Observable<QuarterlyReport[]> {
    return this.http.post<QuarterlyReport[]>(environment.apiURL + "/api/quarterly/delete-report/", qReport );
  }

  getReports(strikeForceID:string): Observable<QuarterlyReport[]> {
    return this.http.get<QuarterlyReport[]>(environment.apiURL + "/api/quarterly/get-reports/" + strikeForceID);
  }

  getReport(quarterlyReportID: string): Observable<QuarterlyReport> {
    return this.http.get<QuarterlyReport>(environment.apiURL + "/api/quarterly/get-report/" + quarterlyReportID);
  }

  getReportByYear(fiscalYear: string, strikeForceID:string): Observable<QuarterlyReport[]> {
    return this.http.get<QuarterlyReport[]>(environment.apiURL + "/api/quarterly/get-year/" + fiscalYear + "/" + strikeForceID);
  }

  getRecentReports(strikeForceID: string): Observable<QuarterlyReport[]> {
    return this.http.get<QuarterlyReport[]>(environment.apiURL + "/api/quarterly/recent/" + strikeForceID);
  }

  addSpecificMeasure(newMeasure:QuarterlyActivity): Observable<QuarterlyActivity> {
    return this.http.post<QuarterlyActivity>(environment.apiURL + "/api/quarterlyactvity/new-specific-measure/", newMeasure);
  }

  deleteSpecificMeasure(newMeasure: QuarterlyActivity): Observable<QuarterlyActivity> {
    return this.http.post<QuarterlyActivity>(environment.apiURL + "/api/quarterlyactvity/delete-specific-measure/", newMeasure);
  }

  addStaffingAgency(newStaff: Staffing): Observable<Staffing> {
    return this.http.post<Staffing>(environment.apiURL + "/api/staffing/new-staff/", newStaff);
  }

  deleteStaffingAgency(delStaff: Staffing): Observable<Staffing> {
    return this.http.post<Staffing>(environment.apiURL + "/api/staffing/delete-staff/", delStaff);
  }

  getStaffing(quarterlyReportID: string): Observable<Staffing[]> {
    return this.http.get<Staffing[]>(environment.apiURL + "/api/staffing/get-staff/" + quarterlyReportID);
  }

  updateMeasures(updateMeasures: QuarterlyActivity[]) {
    return this.http.post<QuarterlyActivity[]>(environment.apiURL + "/api/quarterlyactvity/update-measure/", updateMeasures);
  }

  UpdateStaff(qReport: Staffing[]): Observable<Staffing[]> {
    return this.http.post<Staffing[]>(environment.apiURL + "/api/staffing/update-staff", qReport);
  }

  addNewSeizure(newSeizure: QuarterlyActivity): Observable<QuarterlyActivity> {
    return this.http.post<QuarterlyActivity>(environment.apiURL + "/api/quarterlyactvity/new-seizure-measure/", newSeizure);
  }

  getRequired(quarterlyReportID: string): Observable<QuarterlyActivity[]> {
    return this.http.get<QuarterlyActivity[]>(environment.apiURL + "/api/quarterlyactvity/get-required/" + quarterlyReportID);
  }

  getSpecific(quarterlyReportID: string): Observable<QuarterlyActivity[]> {
    return this.http.get<QuarterlyActivity[]>(environment.apiURL + "/api/quarterlyactvity/get-specific/" + quarterlyReportID);
  }

  getSeizure(quarterlyReportID: string): Observable<QuarterlyActivity[]> {
    return this.http.get<QuarterlyActivity[]>(environment.apiURL + "/api/quarterlyactvity/get-seizure/" + quarterlyReportID);
  }

  getOcdetfOnly(quarterlyReportID: string): Observable<QuarterlyActivity[]> {
    return this.http.get<QuarterlyActivity[]>(environment.apiURL + "/api/quarterlyactvity/get-ocdetf/" + quarterlyReportID);
  }

  addNewRecentCase(newRecentCase: RecentDevelopment): Observable<RecentDevelopment[]> {
    return this.http.post<RecentDevelopment[]>(environment.apiURL + "/api/recent/create-new/", newRecentCase);
  }

  updateNewRecentCase(newRecentCase: RecentDevelopment): Observable<RecentDevelopment[]> {
    return this.http.post<RecentDevelopment[]>(environment.apiURL + "/api/recent/update-case/", newRecentCase);
  }

  getRecentCases(quarterlyReportID: string): Observable<RecentDevelopment[]> {
    return this.http.get<RecentDevelopment[]>(environment.apiURL + "/api/recent/get-all/" + quarterlyReportID);
  }

  deleteRecentCase(delRecentCase: RecentDevelopment): Observable<RecentDevelopment[]> {
    return this.http.post<RecentDevelopment[]>(environment.apiURL + "/api/recent/delete-item/", delRecentCase);
  }

  getSummary(fiscalYear:string, fiscalQuarter: string, table: string): Observable<SummaryAnalysis[]> {
    return this.http.get<SummaryAnalysis[]>(environment.apiURL + "/api/analysis/get-summary/" + table + "/" + fiscalYear + "/" + fiscalQuarter);
  }

  createAnalysisPDF(fiscalYear: string, fiscalQuarter: string) {
    
    return this.http.get<any>(environment.apiURL + "/api/analysis/get-pdf/" + fiscalYear + "/" + fiscalQuarter, { responseType: 'blob' as 'json'});
  }

  validateStaffing(quarterlyReportID: string): Observable<ValidationResult[]> {
    return this.http.get<ValidationResult[]>(environment.apiURL + "/api/staffing/validate/" + quarterlyReportID);
  }

  validateReport(quarterlyReportID: string): Observable<ValidationResult[]> {
    return this.http.get<ValidationResult[]>(environment.apiURL + "/api/quarterly/validate/" + quarterlyReportID);
  }

  validateActivity(quarterlyReportID: string): Observable<ValidationResult[]> {
    return this.http.get<ValidationResult[]>(environment.apiURL + "/api/quarterlyactvity/validate/" + quarterlyReportID);
  }

}
