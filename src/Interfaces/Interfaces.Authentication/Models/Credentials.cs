using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public partial class Credentials : ICredentials
    {
        public string AuthenticationPlugin
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_AuthenticationPlugin))
                    _AuthenticationPlugin = "Any";
                return _AuthenticationPlugin;
            }
            set { _AuthenticationPlugin = value; }
        } private string _AuthenticationPlugin;

        /// <summary>
        /// The Username.
        /// </summary>
        /// <remarks>User names must not start or end with spaces.</remarks>
        public string User
        {
            get { return _User; }
            set { _User = value?.Trim(); }
        } private string _User;

        /// <summary>
        /// The password.
        /// </summary>
        /// <remarks>Password should not be trimmed. Spaces are valid characters.</remarks>
        [ExcludeFromCodeCoverage]
        public string Password { get; set; }
    }
}