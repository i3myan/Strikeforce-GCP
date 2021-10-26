using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.MongoDB
{
    public class MongoDBArchive : IStrikeForceArchive
    {
        private IQuarterlyActivityService quarterlyActivity;
        IQuarterReportService quarterReport;
        IStaffingService staffing;
        IRecentDevelopmentService recent;

        public MongoDBArchive() { }

        public MongoDBArchive(IQuarterlyActivityService quarterlyActivity, IQuarterReportService quarterReport, IStaffingService staffing, IRecentDevelopmentService recent) {
            this.quarterReport = quarterReport;
            this.staffing = staffing;
            this.quarterlyActivity = quarterlyActivity;
            this.recent = recent;
        }



        public bool Archive()
        {
            throw new NotImplementedException();
        }

        public bool ArchiveByReport(string quarterlyReportID)
        {
            throw new NotImplementedException();
        }

        public bool ArchiveByYear(int? year)
        {
            throw new NotImplementedException();
        }
    }
}
