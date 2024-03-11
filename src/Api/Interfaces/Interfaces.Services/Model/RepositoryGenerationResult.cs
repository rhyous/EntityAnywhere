namespace Rhyous.EntityAnywhere.Interfaces
{
    public class RepositoryGenerationResult : IName
    {
        /// <summary>
        /// The name of the entity
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if the repo was generated, false if it wasn't.
        /// </summary>
        public bool RepositoryReady { get; set; }

        /// <summary>
        /// The reason the repo wasn't generated.
        /// This could be an expected reason, such as in the case of a ReadOnlyEntity, or
        /// it could be an exception message.
        /// </summary>
        public string FailureReason { get; set; }

        public bool ShouldSerializeFailureReason()
        {
            return !RepositoryReady;
        }
    }
}