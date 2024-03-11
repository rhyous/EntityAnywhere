using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IDuplicateUsernameDetector
    {
        IEnumerable<string> Detect(IEnumerable<string> usernames, bool throwIfDuplicatesFound = true);
    }
}
