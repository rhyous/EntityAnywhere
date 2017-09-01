using System;
using System.Text;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A class that can create or extract credentials from a BasicAuth header.
    /// </summary>
    public class BasicAuth
    {
        private readonly string _User;
        private readonly string _Password;
        private const string Prefix = "Basic ";

        #region Constructors
        public BasicAuth(string encodedHeader)
            : this(encodedHeader, Encoding.UTF8)
        {
        }

        public BasicAuth(string encodedHeader, Encoding encoding)
        {
            HeaderValue = encodedHeader;
            var decodedHeader = encodedHeader.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)
                ? encoding.GetString(Convert.FromBase64String(encodedHeader.Substring(Prefix.Length)))
                : encoding.GetString(Convert.FromBase64String(encodedHeader));
            var credArray = decodedHeader.Split(':');
            if (credArray.Length > 0)
                _User = credArray[0];
            if (credArray.Length > 1)
                _Password = credArray[1];
        }

        public BasicAuth(string user, string password)
            : this(user, password, Encoding.UTF8)
        {
        }

        public BasicAuth(string user, string password, Encoding encoding)
        {
            _User = user;
            _Password = password;
            HeaderValue = Prefix + Convert.ToBase64String(encoding.GetBytes(string.Format("{0}:{1}", _User, _Password)));
        }
        #endregion

        /// <summary>
        /// The credentials. Either these were provided to the constructor or the constructor generated these from a BasicAuth header.
        /// </summary>
        public Credentials Creds
        {
            get { return _Creds ?? (_Creds = new Credentials { User = _User, Password = _Password }); }
        } private Credentials _Creds;

        /// <summary>
        /// The BasicAuth header. Either this was provided to the constructor or the constructor generated this from a user name and password.
        /// </summary>
        public string HeaderValue { get; }
    }
}
