export class Strikeforcenames {
  ID: string;
  Name: string;
  IsActive: boolean;

  DateCreated: Date;
  DateUpdated: Date;
  CreatedUserID: string;
  UpdatedUserID: string;

  constructor() {
    this.ID = "";
    this.Name = "";
    this.IsActive = false;
    this.DateCreated = new Date();
    this.DateUpdated = new Date();
    this.CreatedUserID = "";
    this.UpdatedUserID = "";
  }
}
