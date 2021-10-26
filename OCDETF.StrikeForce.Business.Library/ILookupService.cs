using OCDETF.StrikeForce.Business.Library.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library
{
    public interface ILookupService
    {
        IList<StrikeForceNames> GetForceLocations();

        StrikeForceNames AddForce(StrikeForceNames forceLocation, string userID);
    }
}
