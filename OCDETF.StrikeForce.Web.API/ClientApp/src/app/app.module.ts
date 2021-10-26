import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HistoryComponent } from './history/history.component';
import { MissionComponent } from './mission/mission.component';
import { StructureComponent } from './structure/structure.component';
import { OfficeLocationsComponent } from './office-locations/office-locations.component';
import { StaffingsComponent } from './staffings/staffings.component';
import { AllocdetfComponent } from './allocdetf/allocdetf.component';
import { OcdetfonlyComponent } from './ocdetfonly/ocdetfonly.component';
import { SeizuresComponent } from './seizures/seizures.component';
import { SpecificComponent } from './specific/specific.component';
import { NotesComponent } from './notes/notes.component';
import { ChallengesComponent } from './challenges/challenges.component';
import { RecentDevelopmentsComponent } from './recent-developments/recent-developments.component';
import { FormsModule } from '@angular/forms';
import { AppConfigurationService } from './app-configuration.service';
import { CKEditorModule } from 'ckeditor4-angular';
import { LoginComponent } from './login/login.component';
import { MyreportsComponent } from './myreports/myreports.component';
import { QuarterlyreportComponent } from './quarterlyreport/quarterlyreport.component';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { InitiateReportComponent } from './initiate-report/initiate-report.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './http.interceptor';
import { SessionService } from './session.service';
import { ToastrModule } from 'ngx-toastr';
import { FocusDirective } from './focus';
import { HeadsupComponent } from './headsup/headsup.component';
import { ReportTableComponent } from './report-table/report-table.component';
import { PrintreportComponent } from './printreport/printreport.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { LogoutComponent } from './logout/logout.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { SummaryComponent } from './summary/summary.component';
import { MatTable, MatTableModule } from '@angular/material/table';
import { MatSlider, MatSliderModule } from '@angular/material/slider';
import { MatInputModule } from '@angular/material/input';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { RedirectComponent } from './redirect/redirect.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MatPaginatorModule } from '@angular/material/paginator';

@NgModule({
  declarations: [
    AppComponent,
    HistoryComponent,
    MissionComponent,
    StructureComponent,
    OfficeLocationsComponent,
    StaffingsComponent,
    AllocdetfComponent,
    OcdetfonlyComponent,
    SeizuresComponent,
    SpecificComponent,
    NotesComponent,
    ChallengesComponent,
    RecentDevelopmentsComponent,
    LoginComponent,
    MyreportsComponent,
    QuarterlyreportComponent,
    HomeComponent,
    AdminComponent,
    InitiateReportComponent,
    FocusDirective,
    HeadsupComponent,
    ReportTableComponent,
    PrintreportComponent,
    LogoutComponent,
    SummaryComponent,
    RedirectComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    CKEditorModule,
    ToastrModule.forRoot(),
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatSliderModule,
    MatIconModule,
    MatSortModule
  ],
  providers: [AppConfigurationService, SessionService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
