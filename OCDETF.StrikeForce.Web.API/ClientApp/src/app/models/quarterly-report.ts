import { QuarterlyActivity } from "./quarterly-activity";
import { RecentDevelopment } from "./recent-development";
import { Staffing } from "./staffing";

export class QuarterlyReport {
  ID: string;
  Name: string;
  StrikeForceName: string;
  StrikeForceID: string;
  FiscalYear: string;
  Quarter: string;
  OperationsBegin: string;
  MOUDate: string;
  HistoryNotes: string;
  Mission?: string;
  Structure?: string;
  OfficeLocations: string;  
  Challenges: string;
  HeadsUp: string;
  SeizureNotes: string;
  SpecificNotes: string;
  IsActive: boolean;
  Status: string;

  CreatedUserID: string;
  UpdatedUserID: string;
  DateCreated: Date;
  DateUpdated: Date;

  PartitionKey: string;
  RowKey: string;

  StaffingData: Staffing[];
  RequiredData: QuarterlyActivity[];
  OCDETFCases: QuarterlyActivity[];
  SeizuresData: QuarterlyActivity[];
  SpecificData: QuarterlyActivity[];
  RecentDevelopments: RecentDevelopment[];

  constructor() {
    this.ID = "";
    this.Name = "";
    this.StrikeForceName = "";
    this.StrikeForceID = "";
    this.FiscalYear = "";
    this.Quarter = "";
    this.OperationsBegin = "";
    this.MOUDate = "";
    this.HistoryNotes = "";
    this.OfficeLocations = "";
    this.StaffingData = [];
    this.RequiredData = [];
    this.OCDETFCases = [];

    this.SeizuresData = [];
    this.SpecificData = [];
    this.SeizureNotes = "";
    this.Challenges = "";
    this.RecentDevelopments = [];
    this.HeadsUp = "";
    this.Status = "";
    this.SpecificNotes = "";
    this.CreatedUserID = "";
    this.UpdatedUserID = "";
    this.DateCreated = new Date();
    this.DateUpdated = new Date();
    this.PartitionKey = "";
    this.RowKey = "";
    this.IsActive = false;
  }
}
