namespace Rhyous.EntityAnywhere.Interfaces
{
    public class UserDetails : IUserDetails
    {
        private readonly IClaimsProvider _ClaimsProvider;

        public UserDetails(IClaimsProvider claimsProvider)
        {
            _ClaimsProvider = claimsProvider;
        }

        public string Username => string.IsNullOrWhiteSpace(_Username) ? (_Username = _ClaimsProvider.GetClaim("User", "Username")) : _Username;
        private string _Username;

        public long UserId => _UserId < 1 ? (_UserId = _ClaimsProvider.GetClaim<long>("User", "Id")) : _UserId;
        private long _UserId;

        public string UserRole => string.IsNullOrWhiteSpace(_UserRole) ? (_UserRole = _ClaimsProvider.GetClaim("UserRole", "Role")) : _UserRole;
        private string _UserRole;
    }
}