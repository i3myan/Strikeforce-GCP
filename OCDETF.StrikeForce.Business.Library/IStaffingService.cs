using OCDETF.StrikeForce.Business.Library.Models;
using System.Collections.Generic;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface IStaffingService
    {
        IList<Staffing> Get(string quarterlyReportID);

        Staffing Add(Staffing newRecord, string userID);
        Staffing Update(Staffing newRecord, string userID);
        Staffing Delete(Staffing updateRecord, string userID);
        IList<Staffing> AddRecords(IList<Staffing> staffRecords, QuarterlyReport newReport, string userID);
        IList<Staffing> UpdateRecords(IList<Staffing> staffRecords, string userID);
        IList<ValidationResult> Validate(string quarterlyReportID);

        IList<Staffing> GetByYear(int fiscalYear);
    }
}
