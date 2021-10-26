using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Core.Library.Audit
{
    public class AuditLog
    {
        public string ID { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }

        public DateTime DateCreated { get; set; }
        public string CreatedUserID { get; set; }
    }
}
