using System;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IAuthenticationSettings
    {
        int MaxFailedAttempts { get; }
        int MaxFailedAttemptsMinutes { get; }
        DateTimeOffset Start { get; }
    }
}