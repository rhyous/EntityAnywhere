using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using System;
using System.Linq;

namespace Rhyous.WebFramework.Authenticators
{
    internal static class OdataObjectExtensions
    {
        public static string GetSapId(this OdataObject<Organization, int> odataOrg)
        {
            var odataAltIdCollection = odataOrg.GetRelatedEntityCollection<AlternateId, long>();
            var altId = odataAltIdCollection?.FirstOrDefault(ai => ai.Object.Property.Equals("SapId", StringComparison.OrdinalIgnoreCase))?.Object.Value
                     ?? odataAltIdCollection?.FirstOrDefault(ai => ai.Object.Property.Equals("S4Id", StringComparison.OrdinalIgnoreCase))?.Object.Value
                     ?? odataOrg.Object.SapId;
            return altId;
        }
    }
}