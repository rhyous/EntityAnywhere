namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The token entity interface. This is used by the Authentication service to provide a token that can be included in the header of subsequent web calls.
    /// </summary>
    public partial interface IToken : IEntity<long>, IAuditable
    {
        /// <summary>
        /// The token text. This must be placed in the header.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// This is the id of the <see cref="IUser"/> that the token references.
        /// </summary>
        long UserId { get; set; }
    }
}
