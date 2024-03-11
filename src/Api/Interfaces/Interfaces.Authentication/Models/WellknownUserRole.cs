namespace Rhyous.EntityAnywhere.Interfaces
{
    public class WellknownUserRole
    {
        public const string Anonymous = nameof(Anonymous);
        public const string Admin = nameof(Admin);
        public const string Publisher = nameof(Publisher);
        public const string Author = nameof(Author);
        public const string Customer = nameof(Customer);
    }

    public class WellknownUserRoleIds
    {
        public const int Anonymous = 0;
        public const int Admin = 1;
        public const int Publisher = 2;
        public const int Author = 3;
        public const int Customer = 4;
    }
}