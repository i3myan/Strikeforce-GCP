using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library
{
    interface IStrikeForceArchive
    {
        bool ArchiveByYear(int? year);

        bool ArchiveByReport(string quarterlyReportID);

        bool Archive();
    }
}
