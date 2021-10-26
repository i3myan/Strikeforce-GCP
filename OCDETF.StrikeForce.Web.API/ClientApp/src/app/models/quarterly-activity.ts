export class QuarterlyActivity {
  ID: string;
  StrikeForceID: string;
  QuarterlyReportID: string;
  ActivityName: string;
  FirstQuarter: string;
  SecondQuarter: string;
  ThirdQuarter: string;
  FourthQuarter: string;
  Category: string;
  Total: string | undefined;
  IsActive: boolean;
  IsCommon: boolean;
  Order: number;

  constructor() {
    this.ID = "";
    this.ActivityName = "";
    this.StrikeForceID = "";
    this.QuarterlyReportID = "";
    this.FirstQuarter = "";
    this.SecondQuarter = "";
    this.ThirdQuarter = "";
    this.FourthQuarter = "";
    this.Category = "";
    this.Total = undefined;
    this.IsActive = false;
    this.IsCommon = false;
    this.Order = 0
  }
}
