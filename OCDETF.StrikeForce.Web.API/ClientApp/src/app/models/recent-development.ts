import { dashCaseToCamelCase } from "@angular/compiler/src/util";

export class RecentDevelopment {
  ID: string;
  StrikeForceID: string;
  QuarterlyReportID: string;
  CaseType: string;
  Order: number;

  AgencyName: string;
  SponsorAgency: string;
  Summary: string;
  IsActive: boolean;




  constructor() {
    this.AgencyName = "";
    this.ID = "";
    this.QuarterlyReportID = "";
    this.CaseType = "";
    this.IsActive = false;
    this.Order = 0;
    this.SponsorAgency = "";
    this.Summary = "";
    this.StrikeForceID = "";
  }
}
