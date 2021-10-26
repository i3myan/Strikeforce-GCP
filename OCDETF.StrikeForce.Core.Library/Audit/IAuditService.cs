using OCDETF.StrikeForce.Core.Library.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Core.Library
{
    public interface  IAuditService
    {
        bool Log(AuditLog log);
    }
}
