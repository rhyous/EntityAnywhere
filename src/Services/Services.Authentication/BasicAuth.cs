using System;
using System.Text;

namespace Rhyous.WebFramework.Services
{
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

        public Credentials Creds
        {
            get { return _Creds ?? (_Creds = new Credentials { User = _User, Password = _Password }); }
        }
        private Credentials _Creds;

        public string HeaderValue { get; }
    }
}
