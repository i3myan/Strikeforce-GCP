using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface IQtrActivityAnalysisService
    {
        IList<SummaryAnalysis> Total(string table, int fiscalYear,int[] fiscalQuarters);
    }
}
