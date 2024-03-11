using Rhyous.Collections;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This class is used to track about to be added entities by AlterateKey, and if two 
    /// simultaneous web service calls have duplicates, they can't both add to this 
    /// ConcurrentHashSet and so the duplicates will be detected.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity</typeparam>
    /// <typeparam name="TAltKey">The type of the alternate key property.</typeparam>
    public class AlternateKeyTracker<TEntity, TAltKey> : ConcurrentHashSet<TAltKey>
    {
    }
}
