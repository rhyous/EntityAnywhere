using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The token entity. This is used by the Authentication service to provide a token that can be included in the header of subsequent web calls.
    /// </summary>
    [AlternateKey("Text")]
    [RelatedEntityExclusions("Addendum", "AlternateId")]
    public partial class Token : AuditableEntity<long>, IToken
    {
        /// <inheritdoc />
        public string Text { get; set; }

        /// <inheritdoc />
        [RelatedEntity("User")]
        public long UserId { get; set; }
        
        /// <inheritdoc />
        [NotMapped]
        public List<ClaimDomain> ClaimDomains
        {
            get { return _ClaimDomains ?? (_ClaimDomains = new List<ClaimDomain>()); }
            set { _ClaimDomains = value; }
        } private List<ClaimDomain> _ClaimDomains;
    }
}
