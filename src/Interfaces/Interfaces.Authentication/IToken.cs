﻿using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The token entity interface. This is used by the Authentication service to provide a token that can be included in the header of subsequent web calls.
    /// </summary>
    public partial interface IToken : IAuditableCreateDate
    {
        /// <summary>
        /// The token text. This must be placed in the header.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// This is the id of the authentication entity, such as but not limited to user, that the token references.
        /// </summary>
        string CredentialEntity { get; set; }

        /// <summary>
        /// This is the id of the authentication entity, such as but not limited to user, that the token references.
        /// </summary>
        long CredentialEntityId { get; set; }

        /// <summary>
        /// The user's role id
        /// </summary>
        int RoleId { get; set; }
        
        /// <summary>
        /// The users role name
        /// </summary>
        string Role { get; set; }

        /// <summary>
        /// A list of claims, separated into each claim domain.
        /// </summary>
        /// <remarks>This is read only. This is calculated on GET and ignored on create verbs such as POST.</remarks>
        List<ClaimDomain> ClaimDomains { get; set; }
    }
}