namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IUserAndPassword
    {
        /// <summary>
        /// The username used to login as this user.
        /// </summary>
        string Username { get; set; }
        /// <summary>
        /// The password used to login as this user.
        /// </summary>
        string Password { get; set; }
    }
}