namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// A User association to an external system.
    /// </summary>
    public interface IUserAssociation
    {
        /// <summary>
        /// An external system the user is associated with, such as Active Directory,
        /// or an SSO system, i.e. Facebook, twitter, linkedin, google, Salesforce.
        /// </summary>
        string System { get; set; }

        /// <summary>
        /// The user id in the external system.
        /// </summary>
        string SystemUserId { get; set; }
    }
}