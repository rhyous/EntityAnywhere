using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    public partial class Token : AuditableEntityBase<long>, IToken
    {
        /// <inheritdoc />
        public string Text { get; set; }
        /// <inheritdoc />
        public long UserId { get; set; }
    }
}
