using Rhyous.WebFramework.Clients2;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.Odata;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Rhyous.WebFramework.Exceptions;

namespace Rhyous.WebFramework.Authenticators
{
    class ClaimsBuilder : IClaimsBuilder
    {
        private readonly IAdminEntityClientAsync<Organization, int> _OrganizationClient;

        public ClaimsBuilder(IAdminEntityClientAsync<Organization, int> organizationClient)
        {
            _OrganizationClient = organizationClient;
        }

        public async Task<List<ClaimDomain>> BuildAsync(IActivationCredential cred)
        {
            if (cred is null) { throw new ArgumentNullException(nameof(cred)); }

            var userClaims = BuildUserClaims(cred.Username, cred.Id.ToString(), DateTimeOffset.Now);
            var orgClaims = await BuildOrganizationClaimAsync(cred.OrganizationId);
            var roleClaims = BuildRolesClaim();
            return new List<ClaimDomain> { userClaims, orgClaims, roleClaims };
        }

        /// <summary>
        /// This builds user claims from an Activation Credentials entity, not from a User entity.
        /// </summary>
        /// <param name="username">The username from the Activation Credentials entity.</param>
        /// <param name="primaryIdentifier">The Id of the Activation Credentials entity.</param>
        /// <returns>A Claim domain with a subject of User.</returns>
        /// <remarks>Activation Credentials is not the User entity, but it is similar, just used only for the Activation role.</remarks>
        public ClaimDomain BuildUserClaims(string username, string primaryIdentifier, DateTimeOffset lastAuthenticated)
        {
            if (string.IsNullOrWhiteSpace(username)) { throw new ArgumentException($"'{nameof(username)}' cannot be null or whitespace.", nameof(username)); }
            if (string.IsNullOrWhiteSpace(primaryIdentifier)) { throw new ArgumentException($"'{nameof(primaryIdentifier)}' cannot be null or whitespace.", nameof(primaryIdentifier)); }

            return new ClaimDomain()
            {
                Claims = new ClaimsList()
                        {
                            // Username
                            new Claim()
                            {
                                Name = "AuthenticationPlugin",
                                Issuer = "LOCAL AUTHORITY",
                                Subject = "User",
                                Value = "ActivationCredentials",
                            },
                            new Claim()
                            {
                                Name = "Username",
                                Issuer = "LOCAL AUTHORITY",
                                Subject = "User",
                                Value = username,
                            },

                            // Id
                            new Claim()
                            {
                                Name = "Id",
                                Issuer = "LOCAL AUTHORITY",
                                Subject = "User",
                                Value = primaryIdentifier
                            },

                            // Last Authenticated
                            new Claim()
                            {
                                Name = "LastAuthenticated",
                                Issuer = "LOCAL AUTHORITY",
                                Subject = "User",
                                Value = lastAuthenticated.ToString(DateTimeFormatInfo.CurrentInfo.RFC1123Pattern)
                            }
                        },
                Issuer = "LOCAL AUTHORITY",
                Subject = "User"

            };
        }

        public async Task<ClaimDomain> BuildOrganizationClaimAsync(int organizationId)
        {
            if (organizationId < 1) { throw new ArgumentException($"'{nameof(organizationId)}' must be a value greater than zero.", nameof(organizationId)); }

            var odataOrg = await _OrganizationClient.GetAsync(organizationId, "$Expand=AlternateId");
            if (odataOrg == null)
                throw new EntityNotFoundException("The supplied activation credential is not associated with an organization");

            var claimDomain = new ClaimDomain()
            {
                Claims = new ClaimsList()
                {
                    new Claim()
                    {
                        Name = "Id",
                        Issuer = "LOCAL AUTHORITY",
                        Subject = "Organization",
                        Value = odataOrg.Id.ToString()
                    },
                    new Claim()
                    {
                        Name = "Name",
                        Issuer = "LOCAL AUTHORITY",
                        Subject = "Organization",
                        Value = odataOrg.Object.Name
                    },
                },
                Issuer = "LOCAL AUTHORITY",
                Subject = "Organization"
            };
            string altId = odataOrg.GetSapId();
            if (!string.IsNullOrWhiteSpace(altId))
            {
                claimDomain.Claims.Add(new Claim()
                {
                    Name = "SapId",
                    Issuer = "LOCAL AUTHORITY",
                    Subject = "Organization",
                    Value = altId
                });
            }
            return claimDomain;
        }

        public ClaimDomain BuildRolesClaim()
        {
            return new ClaimDomain()
            {
                Claims = new ClaimsList()
                {
                    new Claim()
                    {
                        Name = "Role",
                        Issuer = "LOCAL AUTHORITY",
                        Subject = "UserRole",
                        Value = "Activation"
                    }
                },
                Issuer = "LOCAL AUTHORITY",
                Subject = "UserRole"
            };
        }
    }
}