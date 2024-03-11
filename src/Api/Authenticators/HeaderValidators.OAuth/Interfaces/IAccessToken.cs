namespace Rhyous.EntityAnywhere.HeaderValidators
{
    public interface IAccessToken
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        long Expires { get; set; }
        long NotBefore { get; set; }
        string ClientId { get; set; }
        string[] Scope { get; set; }
        string Subject { get; set; }
        long AuthTime { get; set; }
        string IdentityProvider { get; set; }
        long UserId { get; set; }
        int UserRoleId { get; set; }
	}
}
