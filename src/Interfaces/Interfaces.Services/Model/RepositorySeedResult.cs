namespace Rhyous.EntityAnywhere.Interfaces
{
    public class RepositorySeedResult : IName
    {
        /// <summary>
        /// The name of the entity
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the entity has seed data or not.
        /// </summary>
        public bool EntityHasSeedData { get; set; }

        /// <summary>
        /// True if the repo was seeded with entity data, false if it wasn't.
        /// </summary>
        public bool SeedSuccessful { get; set; }

        /// <summary>
        /// The reason the repo wasn't seeded with entity data.
        /// This could be an expected reason, such as in the case of a ReadOnlyEntity, or
        /// it could be an exception message.
        /// </summary>
        public string FailureReason { get; set; }

        public bool ShouldSerializeFailureReason()
        {
            return !SeedSuccessful;
        }
    }
}