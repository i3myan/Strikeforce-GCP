import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoryComponent } from './history/history.component';
import { StaffingsComponent } from './staffings/staffings.component';
import { SeizuresComponent } from './seizures/seizures.component';
import { SpecificComponent } from './specific/specific.component';
import { RecentDevelopmentsComponent } from './recent-developments/recent-developments.component';
import { ChallengesComponent } from './challenges/challenges.component';
import { OcdetfonlyComponent } from './ocdetfonly/ocdetfonly.component';
import { AllocdetfComponent } from './allocdetf/allocdetf.component';
import { MissionComponent } from './mission/mission.component';
import { StructureComponent } from './structure/structure.component';
import { NotesComponent } from './notes/notes.component';
import { OfficeLocationsComponent } from './office-locations/office-locations.component';
import { LoginComponent } from './login/login.component';
import { MyreportsComponent } from './myreports/myreports.component';
import { QuarterlyreportComponent } from './quarterlyreport/quarterlyreport.component';
import { HomeComponent } from './home/home.component';
import { AdminComponent } from './admin/admin.component';
import { InitiateReportComponent } from './initiate-report/initiate-report.component';
import { AuthGuard } from './auth.guard';
import { HeadsupComponent } from './headsup/headsup.component';
import { PrintreportComponent } from './printreport/printreport.component';
import { SummaryComponent } from './summary/summary.component';
import { RedirectComponent } from './redirect/redirect.component';
import { DashboardComponent } from './dashboard/dashboard.component';


const routes: Routes = [
  { path: '', component: LoginComponent }
  , { path: 'login', component: LoginComponent }
  , { path: 'admin', component: AdminComponent, canActivate: [AuthGuard] }
  , { path: 'myreports', component: MyreportsComponent, canActivate: [AuthGuard] }
  , { path: 'initiate', component: InitiateReportComponent, canActivate: [AuthGuard] }
  , { path: 'print/:id/:return', component: PrintreportComponent, canActivate: [AuthGuard] }
  , { path: 'home', component: HomeComponent, canActivate: [AuthGuard] }
  , { path: 'analysis', component: SummaryComponent, canActivate: [AuthGuard] }
  , { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] }
  , { path: 'redirect/:id/:section', component:RedirectComponent, canActivate:[AuthGuard]}
  , {
    path: 'quarterly/:id',
    component: QuarterlyreportComponent,
    children: [
      { path: 'history', component: HistoryComponent, canActivate: [AuthGuard]  }
      , { path: 'locations', component: OfficeLocationsComponent, canActivate: [AuthGuard]  }
      , { path: 'staffings', component: StaffingsComponent, canActivate: [AuthGuard]  }
      , { path: 'seizures', component: SeizuresComponent, canActivate: [AuthGuard]  }
      , { path: 'recent', component: RecentDevelopmentsComponent, canActivate: [AuthGuard]  }
      , { path: 'challenges', component: ChallengesComponent, canActivate: [AuthGuard]  }
      , { path: 'allocdetf', component: AllocdetfComponent, canActivate: [AuthGuard]  }
      , { path: 'ocdetfonly', component: OcdetfonlyComponent, canActivate: [AuthGuard]  }
      , { path: 'mission', component: MissionComponent, canActivate: [AuthGuard]  }
      , { path: 'structure', component: StructureComponent, canActivate: [AuthGuard]  }
      , { path: 'headsup', component: HeadsupComponent, canActivate: [AuthGuard]  }
      , { path: 'specific', component: SpecificComponent, canActivate: [AuthGuard]  }
    ]}];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash:true, onSameUrlNavigation:'reload'} )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
