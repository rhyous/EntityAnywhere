namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The UserType entity. This stores the type of user: System, Internal, Partner, Customer, Organization, Group, etc.
    /// </summary>
    public partial interface IUserType : IBaseEntity<int>, IAuditable
    {
        string Type { get; set; }
    }
}
