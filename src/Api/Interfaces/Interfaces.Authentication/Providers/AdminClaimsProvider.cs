using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class AdminClaimsProvider : IAdminClaimsProvider
    {
        public List<ClaimDomain> ClaimDomains => CreateAdminClaims(); // Create it new every time, so LastAuthenticated is updated.
        private List<ClaimDomain> CreateAdminClaims()
        {
            return new List<ClaimDomain>
            {
                new ClaimDomain
                {
                    Subject = "Organization",
                    Claims = new ClaimsList
                    {
                        new Claim
                        {
                            Domain = new ClaimDomain(),
                            Subject = "Organization",
                            Issuer = "LOCAL AUTHORITY",
                            Name = "Id",
                            Value = "1",
                        }
                    },
                    Issuer = "LOCAL AUTHORITY",
                },
                new ClaimDomain
                {
                    Subject = "User",
                    Claims = new ClaimsList
                    {
                        new Claim
                        {
                            Domain = new ClaimDomain(),
                            Subject = "User",
                            Issuer = "LOCAL AUTHORITY",
                            Name = "Id",
                            Value = "1",
                        },
                        new Claim
                        {
                            Domain = new ClaimDomain(),
                            Subject = "User",
                            Issuer = "LOCAL AUTHORITY",
                            Name = "Username",
                            Value = "system",
                        },
                        new Claim
                        {
                            Domain = new ClaimDomain(),
                            Subject = "User",
                            Issuer = "LOCAL AUTHORITY",
                            Name = "LastAuthenticated",
                            Value = DateTimeOffset.Now.ToString()
                        }
                    },
                    Issuer = "LOCAL AUTHORITY",
                },
                new ClaimDomain
                {
                    Subject = "UserRole",
                    Claims = new ClaimsList
                    {
                        new Claim
                        {
                            Domain = new ClaimDomain(),
                            Subject = "UserRole",
                            Issuer = "LOCAL AUTHORITY",
                            Name = "Role",
                            Value = WellknownUserRole.Admin
                        }
                    },
                    Issuer = "LOCAL AUTHORITY",
                }
            };
        }
    }
}
