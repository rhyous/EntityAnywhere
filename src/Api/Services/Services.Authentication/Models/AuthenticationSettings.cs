using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// The default settings allow for locking a username if there have been 3 failed attempts in 60 minutes.
    /// </summary>
    public class AuthenticationSettings : IAuthenticationSettings
    {
        internal const string MaxFailedAttemptsKey = "MaxFailedAttempts";
        internal const int MaxFailedAttemptsDefaultValue = 3;
        internal const string MaxFailedAttemptsMinutesKey = "MaxFailedAttemptsMinutes";
        internal const int MaxFailedAttemptsMinutesDefaultValue = 60;

        private readonly NameValueCollection _AppSettings;

        public AuthenticationSettings(IAppSettings appSettings)
        {
            _AppSettings = appSettings?.Collection;
        }

        public int MaxFailedAttempts
        {
            get { return _MaxFailedAttempts ?? (_MaxFailedAttempts = _AppSettings.Get(MaxFailedAttemptsKey, MaxFailedAttemptsDefaultValue)).Value; }
        } private int? _MaxFailedAttempts;

        public int MaxFailedAttemptsMinutes
        {
            get { return _MaxFailedAttemptsTimespan ?? (_MaxFailedAttemptsTimespan = _AppSettings.Get(MaxFailedAttemptsMinutesKey, MaxFailedAttemptsMinutesDefaultValue)).Value; }
        } private int? _MaxFailedAttemptsTimespan;

        public DateTimeOffset Start { get { return DateTimeOffset.Now.AddMinutes(-MaxFailedAttemptsMinutes); } }
    }
}