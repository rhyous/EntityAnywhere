//---------------------------------------------------------------------------
// Copyright (C) Ivanti Corporation 2020. All rights reserved.
//---------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    [DataContract]
    [ExcludeFromCodeCoverage]
    public class AccessToken : IAccessToken
    {
        [DataMember(Name = "iss")]
        public string Issuer { get; set; }

        [DataMember(Name = "aud")]
        public string Audience { get; set; }

        [DataMember(Name = "exp")]
        public long Expires { get; set; }

        [DataMember(Name = "nbf")]
        public long NotBefore { get; set; }

        [DataMember(Name = "client_id")]
        public string ClientId { get; set; }

        [DataMember(Name = "scope", IsRequired = false)]
        public string[] Scope { get; set; }

        [DataMember(Name = "sub")]
        public string Subject { get; set; }

        [DataMember(Name = "auth_time")]
        public long AuthTime { get; set; }

        [DataMember(Name = "idp")]
        public string IdentityProvider { get; set; }

        [DataMember(Name = "user_id")]
        public long UserId { get; set; }

        public int UserRoleId { get; set; }
    }
}
