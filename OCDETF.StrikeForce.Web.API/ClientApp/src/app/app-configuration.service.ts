import { Injectable } from '@angular/core';
import { QuarterlyActivity } from './models/quarterly-activity';
import { RecentDevelopment } from './models/recent-development';
import { Staffing } from './models/staffing';

@Injectable({
  providedIn: 'root'
})
export class AppConfigurationService {

  constructor() { }

  //Staffing Data
  private staffingData: Staffing[] = [
  ];

  //Required information (OCDETF and non-OCDETF cases) 
  private allCases:QuarterlyActivity[] = [
  ]

  //OCDETF Cases Only(data supplied by OCDETF Executive Office)*
  private ocdetfCasesOnly:QuarterlyActivity[] = [   ];

  //Seizures (OCDETF and non-OCDETF cases)
  private seizureData:QuarterlyActivity[] = [  ];

  //Strike Force Specific Information
  private specificInformation:QuarterlyActivity[] = [
  ];

  //Recent Case Developments
  private recentDevelopment:RecentDevelopment[] = [
  ]

  

  getClonedRecentDevelopments(): RecentDevelopment[] {
    let aCopy: RecentDevelopment[] = [];
    this.recentDevelopment.forEach(
      (value, index) => {
        let temp = new RecentDevelopment();
        temp.ID = value.ID;
        temp.AgencyName = value.AgencyName;
        temp.SponsorAgency = value.SponsorAgency;
        temp.Summary = value.Summary;
        aCopy.push(temp);
      });
    return aCopy;
  }
}
