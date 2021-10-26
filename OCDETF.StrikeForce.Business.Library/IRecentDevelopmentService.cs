using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface IRecentDevelopmentService
    {
        IList<RecentDevelopments> Get(string quarterlyReportID);

        IList<RecentDevelopments> Add(RecentDevelopments newRecord, string userID);
        IList<RecentDevelopments> Update(RecentDevelopments newRecord, string userID);
        IList<RecentDevelopments> Delete(RecentDevelopments updateRecord, string userID);
        IList<RecentDevelopments> AddRecords(IList<RecentDevelopments> recentRecords, QuarterlyReport newReport, string userID);
        IList<RecentDevelopments> UpdateRecords(IList<RecentDevelopments> recentRecords,QuarterlyReport newReport, string userID);
        IList<ValidationResult> Validate(string quarterlyReportID);
    }
}
