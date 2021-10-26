using Google.Cloud.Datastore.V1;
using OCDETF.StrikeForce.Business.Library.Models;
using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library
{
    public static class DatastoreExtensionMethods
    {
        public static string ToId(this Key key) => key.Path.First().Id.ToString();

        /// <summary>
        /// Map from Id to Key
        /// </summary>
        /// <param name="id">An Item's id.</param>
        /// <returns>A datastore key.</returns>
        public static Key ToKey(this long id, string kind) =>
            new Key().WithElement(kind, id);

        #region for Strikeforce Location

        /// <summary>
        /// map from datastore Entity to Staffing
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>An Item.</returns>
        public static StrikeForceNames ToStrikeForceNames(this Entity entity) => new StrikeForceNames()
        {
            ID = entity.Key.ToId(),        
            Name = (string)entity["Name"],        
            IsActive = (bool)entity["IsActive"],        
            CreatedUserID = (string)entity["CreatedUserID"],
            UpdatedUserID = (string)entity["UpdatedUserID"],
            DateCreated = (DateTime)entity["DateCreated"],
            DateUpdated = (DateTime)entity["DateUpdated"],
        };

        /// <summary>
        /// Map from StrikeForceNames to Entity
        /// </summary>
        /// <param name="StrikeForceNames">Map StrikeForceNames to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        public static Entity ToStrikeForceNamesEntity(this StrikeForceNames sfName, DatastoreDb db)
        {
            Key key;
            if (String.IsNullOrEmpty(sfName.ID))
            {
                KeyFactory kf = db.CreateKeyFactory(StrikeForceTables.StrikeForceLocations.ToString());
                key = kf.CreateIncompleteKey();
            }
            else
            {
                key = long.Parse(sfName.ID).ToKey(StrikeForceTables.StrikeForceLocations.ToString());
            }
            Entity newEntity = new Entity()
            {
                Key = key,
                ["Name"] = sfName.Name,
                ["IsActive"] = sfName.IsActive,             
                ["CreatedUserID"] = sfName.CreatedUserID,
                ["UpdatedUserID"] = sfName.UpdatedUserID,
                ["DateCreated"] = sfName.DateCreated.ToUniversalTime(),
                ["DateUpdated"] = sfName.DateUpdated.ToUniversalTime()
            };

            return newEntity;
        }

        #endregion

        #region for User 
        /// <summary>
        /// map from datastore Entity to User
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>An Item.</returns>
        public static User ToUser(this Entity entity) => new User()
        {
            ID = entity.Key.ToId(),
            StrikeForceID = (string)entity["StrikeForceID"],
            LastName = (string)entity["LastName"],
            FirstName = (string)entity["FirstName"],
            Email = (string)entity["Email"],
            Password = (string)entity["Password"],
            Contributor = (bool)entity["Contributor"],
            Administrator = (bool)entity["Administrator"],
            Owner = (bool)entity["Owner"],
            LastLoginDate = (DateTime)entity["LastLoginDate"],
            LastLoginIPAddress = (string)entity["LastLoginIPAddress"],
            DateCreated = (DateTime)entity["DateCreated"],
            DateUpdated = (DateTime)entity["DateUpdated"],
            IsActive = (bool)entity["IsActive"],
            CreatedUserID = (string)entity["CreatedUserID"],
            UpdatedUserID = (string)entity["UpdatedUserID"]
        };

        /// <summary>
        /// Map from User to Entity
        /// </summary>
        /// <param name="user">The user to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        public static Entity ToUserEntity(this User user, DatastoreDb db) 
        {
            Key userKey;
            if(String.IsNullOrEmpty(user.ID))
            {
                KeyFactory kf = db.CreateKeyFactory(StrikeForceTables.Users.ToString());
                userKey = kf.CreateIncompleteKey();
            }
            else
            {
                userKey = long.Parse(user.ID).ToUserKey();
            }
            Entity newEntity = new  Entity() {
                Key = userKey,
                ["StrikeForceID"] = user.StrikeForceID,
                ["LastName"] = user.LastName,
                ["FirstName"] = user.FirstName,
                ["Email"] = user.Email,
                ["Password"] = user.Password,
                ["Contributor"] = user.Contributor,
                ["Administrator"] = user.Administrator,
                ["Owner"] = user.Owner,
                ["LastLoginDate"] = user.LastLoginDate.ToUniversalTime(),
                ["LastLoginIPAddress"] = user.LastLoginIPAddress,
                ["DateCreated"] = user.DateCreated.ToUniversalTime(),
                ["DateUpdated"] = user.DateUpdated.ToUniversalTime(),
                ["IsActive"] = user.IsActive,
                ["CreatedUserID"] = user.CreatedUserID,
                ["UpdatedUserID"] = user.UpdatedUserID
            };

            return newEntity;
         
        }

        /// <summary>
        /// Map from Key to Id
        /// </summary>
        /// <param name="key">A datastore key</param>
        /// <returns>A item id.</returns>
        public static string ToUserId(this Key key) => key.Path.First().Id.ToString();

        /// <summary>
        /// Map from Id to Key
        /// </summary>
        /// <param name="id">An Item's id.</param>
        /// <returns>A datastore key.</returns>
        public static Key ToUserKey(this long id) =>
            new Key().WithElement(StrikeForceTables.Users.ToString(), id);

        #endregion

        #region for QuarterReport

        /// <summary>
        /// map from datastore Entity to QuarterlyReport
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>An Item.</returns>
        public static QuarterlyReport ToQuarterlyReport(this Entity entity)
        {
            QuarterlyReport qr = new QuarterlyReport();
            qr.ID = entity.Key.ToId();
            qr.Name = (string)entity["Name"];
            qr.StrikeForceID = (string)entity["StrikeForceID"];
            qr.StrikeForceName = (string)entity["StrikeForceName"];
            qr.Quarter = (int)entity["Quarter"];
            qr.FiscalYear = (int)entity["FiscalYear"];
            qr.OperationsBegin = (string)entity["OperationsBegin"];
            qr.MOUDate = (string) entity["MOUDate"];
            qr.HistoryNotes = (string)entity["HistoryNotes"];
            qr.Mission = (string)entity["Mission"];
            qr.Structure = (string)entity["Structure"];
            qr.OfficeLocations = (string)entity["OfficeLocations"];
            qr.Challenges = (string)entity["Challenges"];
            qr.HeadsUp = (string)entity["HeadsUp"];
            qr.SeizureNotes = (string)entity["SeizureNotes"];
            qr.SpecificNotes = (string)entity["SpecificNotes"];
            qr.IsActive = (bool)entity["IsActive"];
            qr.Status = (string)entity["Status"];   
            qr.CreatedUserID = (string)entity["CreatedUserID"];
            qr.UpdatedUserID = (string)entity["UpdatedUserID"];
            qr.DateCreated = (DateTime)entity["DateCreated"];
            qr.DateUpdated = (DateTime)entity["DateUpdated"];

            return qr;
        }

        /// <summary>
        /// Map from quarterlyReport to Entity
        /// </summary>
        /// <param name="qr">The quarterlyReport to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        public static Entity ToQuarterlyReportEntity(this QuarterlyReport qr, DatastoreDb db)
        {
            Key key;
            if (String.IsNullOrEmpty(qr.ID))
            {
                KeyFactory kf = db.CreateKeyFactory(StrikeForceTables.QuarterlyReports.ToString());
                key = kf.CreateIncompleteKey();
            }
            else
            {
                key = long.Parse(qr.ID).ToKey(StrikeForceTables.QuarterlyReports.ToString());
            }
            Entity newEntity = new Entity()
            {
                Key = key,
                ["Name"] = qr.Name,
                ["StrikeForceID"] = qr.StrikeForceID,
                ["StrikeForceName"] = qr.StrikeForceName,
                ["Quarter"] = qr.Quarter,
                ["FiscalYear"] = qr.FiscalYear,
                 ["OperationsBegin"] = qr.OperationsBegin,
                ["MOUDate"] = qr.MOUDate,
                ["HistoryNotes"] = qr.HistoryNotes,
                ["Mission"] = qr.Mission,
                ["Structure"] = qr.Structure,
                ["OfficeLocations"] = qr.OfficeLocations,
                ["Challenges"] = qr.Challenges,
                ["HeadsUp"] = qr.HeadsUp,
                ["SeizureNotes"] = qr.SeizureNotes,
                ["SpecificNotes"] = qr.SpecificNotes,
                ["IsActive"] = qr.IsActive,
                ["Status"] = qr.Status,
                ["CreatedUserID"] = qr.CreatedUserID,
                ["UpdatedUserID"] = qr.UpdatedUserID,
                ["DateCreated"] = qr.DateCreated.ToUniversalTime(),
                ["DateUpdated"] = qr.DateUpdated.ToUniversalTime()           
            };

            return newEntity;
        }

        #endregion

        #region For Staffing
        /// <summary>
        /// map from datastore Entity to Staffing
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>An Item.</returns>
        public static Staffing Tostaffing(this Entity entity)
        {

            Staffing s = new Staffing();
            s.ID = entity.Key.ToId();
           
            s.StrikeForceID = (string)entity["StrikeForceID"];
            s.QuarterlyReportID = (string)entity["QuarterlyReportID"];
            s.AgencyName = (string)entity["AgencyName"];
            s.NumberOfAgents = (int?)entity["NumberOfAgents"];
            s.NumberOfFederalTFOs = (int?)entity["NumberOfFederalTFOs"];
            s.NumberOfAnalysts = (int?)entity["NumberOfAnalysts"];
            s.OtherNumbers = (int?)entity["OtherNumbers"];
            s.Total = (int?)entity["Total"];
            s.Order = (int)(entity["Order"] ?? 0);
            s.IsActive = (bool) (entity["IsActive"] ?? false);
            s.IsCommon = (bool) (entity["IsCommon"] ?? false);      
            s.CreatedUserID = (string)entity["CreatedUserID"];
            s.UpdatedUserID = (string)entity["UpdatedUserID"];
            s.DateCreated = (DateTime)(entity["DateCreated"] ?? DateTime.UtcNow);
            s.DateUpdated = (DateTime) (entity["DateUpdated"] ?? DateTime.UtcNow);

            return s;
        }

        /// <summary>
        /// Map from Staffing to Entity
        /// </summary>
        /// <param name="Staffing">The staffing to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        public static Entity ToStaffingEntity(this Staffing staffing, DatastoreDb db)
        {
            Key key;
            if (String.IsNullOrEmpty(staffing.ID))
            {
                KeyFactory kf = db.CreateKeyFactory(StrikeForceTables.Staffing.ToString());
                key = kf.CreateIncompleteKey();
            }
            else
            {
                key = long.Parse(staffing.ID).ToKey(StrikeForceTables.Staffing.ToString());
            }
            Entity newEntity = new Entity()
            {
                Key = key,           
                ["StrikeForceID"] = staffing.StrikeForceID,
                ["QuarterlyReportID"] = staffing.QuarterlyReportID,
                ["AgencyName"] = staffing.AgencyName,
                ["NumberOfAgents"] = staffing.NumberOfAgents,
                ["NumberOfFederalTFOs"] = staffing.NumberOfFederalTFOs,
                ["NumberOfAnalysts"] = staffing.NumberOfAnalysts,
                ["OtherNumbers"] = staffing.OtherNumbers,
                ["Total"] = staffing.Total,
                ["Order"] = staffing.Order,
                ["IsActive"] = staffing.IsActive,
                ["IsCommon"] = staffing.IsCommon,             
                ["CreatedUserID"] = staffing.CreatedUserID,
                ["UpdatedUserID"] = staffing.UpdatedUserID,
                ["DateCreated"] = staffing.DateCreated.ToUniversalTime(),
                ["DateUpdated"] = staffing.DateUpdated.ToUniversalTime()
            };

            return newEntity;
        }


        #endregion

        #region For Recent Development
        /// <summary>
        /// map from datastore Entity to RecentDevelopment
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>An Item.</returns>
        public static RecentDevelopments ToRecentDevelopments(this Entity entity) => new RecentDevelopments()
        {
            ID = entity.Key.ToId(),
            StrikeForceID = (string)entity["StrikeForceID"],
            QuarterlyReportID = (string)entity["QuarterlyReportID"],
            CaseType = (string)entity["CaseType"],
            AgencyName = (string)entity["AgencyName"],
            SponsorAgency = (string)entity["SponsorAgency"],
            Summary = (string)entity["Summary"],
            Order = (int)entity["Order"],         
            IsActive = (bool)entity["IsActive"],
            CreatedUserID = (string)entity["CreatedUserID"],
            UpdatedUserID = (string)entity["UpdatedUserID"],
            DateCreated = (DateTime)entity["DateCreated"],
            DateUpdated = (DateTime)entity["DateUpdated"],
        };

        /// <summary>
        /// Map from RecentDevelopments to Entity
        /// </summary>
        /// <param name="RecentDevelopments">The RecentDevelopments to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        public static Entity ToRecentDevelopmentsEntity(this RecentDevelopments dev, DatastoreDb db)
        {
            Key key;
            if (String.IsNullOrEmpty(dev.ID))
            {
                KeyFactory kf = db.CreateKeyFactory(StrikeForceTables.RecentDevelopments.ToString());
                key = kf.CreateIncompleteKey();
            }
            else
            {
                key = long.Parse(dev.ID).ToKey(StrikeForceTables.RecentDevelopments.ToString());
            }
            Entity newEntity = new Entity()
            {
                Key = key,
                ["StrikeForceID"] = dev.StrikeForceID,
                ["QuarterlyReportID"] = dev.QuarterlyReportID,
                ["AgencyName"] = dev.AgencyName,
                ["CaseType"] = dev.CaseType,
                ["SponsorAgency"] = dev.SponsorAgency,
                ["Summary"] = dev.Summary,             
                ["Order"] = dev.Order,
                ["IsActive"] = dev.IsActive,
                ["CreatedUserID"] = dev.CreatedUserID,
                ["UpdatedUserID"] = dev.UpdatedUserID,
                ["DateCreated"] = dev.DateCreated.ToUniversalTime(),
                ["DateUpdated"] = dev.DateUpdated.ToUniversalTime()
            };

            return newEntity;
        }

        #endregion

        #region QuarterlyActivities

        /// <summary>
        /// map from datastore Entity to QuarterlyActivity
        /// </summary>
        /// <param name="entity">An entity retrieved from datastore.</param>
        /// <returns>QuarterlyActivity</returns>
        public static QuarterlyActivity ToQuarterlyActivity(this Entity entity)
        {
            QuarterlyActivity qa = new QuarterlyActivity();

            qa.ID = entity.Key.ToId();
            qa.StrikeForceID = (string)entity["StrikeForceID"];
            qa.QuarterlyReportID = (string)entity["QuarterlyReportID"];
            qa.ActivityName = (string)entity["ActivityName"];          
            qa.FirstQuarter = (string)entity["FirstQuarter"];
            qa.SecondQuarter = (string)entity["SecondQuarter"];
            qa.ThirdQuarter = (string)entity["ThirdQuarter"];
            qa.FourthQuarter = (string)entity["FourthQuarter"];
            qa.Total = (string)entity["Total"];
            qa.Category = (string)entity["Category"];          
            qa.Order = (int)entity["Order"];
            qa.IsCommon = (bool)entity["IsCommon"];
            qa.IsActive = (bool)entity["IsActive"];
            qa.CreatedUserID = (string)entity["CreatedUserID"];
            qa.UpdatedUserID = (string)entity["UpdatedUserID"];
            qa.DateCreated = (DateTime)entity["DateCreated"];
            qa.DateUpdated = (DateTime)entity["DateUpdated"];

            return qa;
        }

        /// <summary>
        /// Map from QuarterlyActivity to Entity
        /// </summary>
        /// <param name="QuarterlyActivity">The QuarterlyActivity to store in datastore.</param>
        /// <returns>A datastore entity.</returns>
        public static Entity ToQuarterlyActivityEntity(this QuarterlyActivity qa, DatastoreDb db, string kind)
        {
            Key key;
            if (String.IsNullOrEmpty(qa.ID))
            {
                KeyFactory kf = db.CreateKeyFactory(kind);
                key = kf.CreateIncompleteKey();
            }
            else
            {
                key = long.Parse(qa.ID).ToKey(kind);
            }
            Entity newEntity = new Entity()
            {
                Key = key,
                ["StrikeForceID"] = qa.StrikeForceID,
                ["QuarterlyReportID"] = qa.QuarterlyReportID,
                ["ActivityName"] = qa.ActivityName,
                ["FirstQuarter"] = qa.FirstQuarter,
                ["SecondQuarter"] = qa.SecondQuarter,
                ["ThirdQuarter"] = qa.ThirdQuarter,
                ["FourthQuarter"] = qa.FourthQuarter,
                ["Total"] = qa.Total,
                ["Category"] = qa.Category,
                ["Order"] = qa.Order,
                ["IsCommon"] = qa.IsCommon,
                ["IsActive"] = qa.IsActive,
                ["CreatedUserID"] = qa.CreatedUserID,
                ["UpdatedUserID"] = qa.UpdatedUserID,
                ["DateCreated"] = qa.DateCreated.ToUniversalTime(),
                ["DateUpdated"] = qa.DateUpdated.ToUniversalTime()
            };

            return newEntity;
        }


        #endregion
    }
}
