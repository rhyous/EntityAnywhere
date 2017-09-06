using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The token entity. This is used by the Authentication service to provide a token that can be included in the header of subsequent web calls.
    /// </summary>
    public partial class Token : AuditableEntity<long>, IToken
    {
        /// <inheritdoc />
        public string Text { get; set; }
        /// <inheritdoc />
        public long UserId { get; set; }
    }
}
