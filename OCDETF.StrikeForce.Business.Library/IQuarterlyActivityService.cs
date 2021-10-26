using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface IQuarterlyActivityService
    {
        /// <summary>
        /// Add a new Quarterly Activity Record.
        /// </summary>
        /// <param name="newRecord"></param>
        /// <param name="userID"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        QuarterlyActivity Add(QuarterlyActivity newRecord, string userID, string table);
        IList<QuarterlyActivity> Update(QuarterlyActivity newRecord, string userID, string table);
        QuarterlyActivity Delete(QuarterlyActivity newRecord, string userID, string table);
        IList<QuarterlyActivity> AddNewRecords(IList<QuarterlyActivity> requiredData, QuarterlyReport newReport, string userID, StrikeForceTables table);
        IList<QuarterlyActivity> UpdateRecords(IList<QuarterlyActivity> requiredData, string userID, string table);
        IList<QuarterlyActivity> Get(string quarterlyReportID, string table);
        IList<QuarterlyActivity> Get(string table);
        IList<ValidationResult> Validate(string quarterlyReportID);
    }
}
