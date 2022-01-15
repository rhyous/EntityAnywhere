using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class AnonymousAllowedUrls : IAnonymousAllowedUrls
    {
        public HashSet<string> Urls => _Urls ?? (_Urls = GetUrls());
        private HashSet<string> _Urls;

        private HashSet<string> GetUrls()
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "/entitlementservices/licenseddeviceservice.svc/getdeactivationreasons",
                "/productgroupmembershipservice.svc/productgroupmemberships" ,
                "/productgroupservice.svc/productgroups",
                "/productservice.svc/products",
                "/singlesignonservice.svc/validatetoken",
                "/Service/$MetaData",
                "/ResActivationService.svc/ResActivations",
                "/DecentralizedIdentityService.svc/ILSManuallyRegister"
            };
        }
    }
}