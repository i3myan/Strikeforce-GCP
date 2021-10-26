export class Staffing {

  ID: string;
  QuarterlyReportID: string;
  StrikeForceID: string;
  AgencyName: string;
  NumberOfAgents?: number;
  NumberOfFederalTFOs?: number;
  NumberOfAnalysts?: number;
  OtherNumbers?: number;
  IsCommon: boolean;
  IsActive: boolean;
  Total?: number;
  Order: number;

  constructor() {
    this.ID = "";
    this.QuarterlyReportID = "";
    this.StrikeForceID = "";
    this.AgencyName = "";
    this.NumberOfAgents = undefined;
    this.NumberOfFederalTFOs = undefined;
    this.NumberOfAnalysts = undefined;
    this.OtherNumbers = undefined;
    this.Total = undefined;
    this.Order = 0;
    this.IsActive = false;
    this.IsCommon = false;
  }

}
