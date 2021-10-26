export class User {

  ID: string;
  StrikeForceID: string;
  Email: string;
  Password: string;
  Administrator: boolean;
  Owner: boolean;
  Contributor: boolean;
  FirstName: string;
  LastName: string;
  LastLoginDate: Date;
  LastLoginIPAddress: string;
  IsActive: boolean;

  DateCreated: Date;
  DateUpdated: Date;
  CreatedUserID: string;
  UpdatedUserID: string;

  Session: string;
  IsAuthenticated: boolean;
  StrikeForceName: string;

  constructor() {
    this.ID = "";
    this.FirstName = "";
    this.LastName = "";
    this.StrikeForceID = "";
    this.Email = "";
    this.Password = "";
    this.Administrator = false;
    this.Contributor = false;
    this.Owner = false;
    this.Session = "";
    this.IsAuthenticated = false;
    this.StrikeForceName = "";

    this.LastLoginDate = new Date();
    this.LastLoginIPAddress = "";
    this.IsActive = false;
    this.DateCreated = new Date();
    this.DateUpdated = new Date();
    this.CreatedUserID = "";
    this.UpdatedUserID = "";
  }

}

