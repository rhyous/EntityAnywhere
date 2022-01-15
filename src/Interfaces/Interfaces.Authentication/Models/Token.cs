using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The token model. This is not an entity and is not stored.
    /// This is used by the Authentication service to provide a token that can be included in the header of subsequent web calls.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class Token : IToken
    {
        /// <inheritdoc />
        [Required]
        public string Text { get; set; }

        public string CredentialEntity { get; set; }

        /// <inheritdoc />
        public long CredentialEntityId { get; set; }

        /// <inheritdoc />
        public int RoleId { get; set; }

        /// <inheritdoc />
        public string Role { get; set; }

        /// <inheritdoc />
        [NotMapped]
        public List<ClaimDomain> ClaimDomains
        {
            get { return _ClaimDomains ?? (_ClaimDomains = new List<ClaimDomain>()); }
            set { _ClaimDomains = value; }
        } private List<ClaimDomain> _ClaimDomains;

        public DateTimeOffset CreateDate { get; set; }
    }
}