namespace Rhyous.EntityAnywhere.Interfaces
{
    public class WellknownUserRole
    {
        public const string Admin = nameof(Admin);
        public const string Customer = nameof(Customer);
        public const string Activation = nameof(Activation);
        public const string InternalCustomer = nameof(InternalCustomer);
    }

    public class WellknownUserRoleIds
    {

        public const int Admin = 1;
        public const int Customer = 2;
        public const int Activation = 3;
        public const int InternalCustomer = 4;
    }
}