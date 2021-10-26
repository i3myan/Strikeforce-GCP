using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface IQuarterReportService
    {
        QuarterlyReport Create(QuarterlyReport newReport, string userID);

        QuarterlyReport Update(QuarterlyReport newReport, string userID);

        IList<QuarterlyReport> Delete(QuarterlyReport deleteReport, string userID);

        IList<QuarterlyReport> GetReports(string strikeForceID, bool isAdmin);

        QuarterlyReport GetReport(string quarterlyReportID, string strikeForceID, bool isAdmin);

        IList<QuarterlyReport> GetReportsByYear(int fiscalYear, string strikeForceID);

        IList<ValidationResult> Validate(string quaterlyReportID);
    }
}
