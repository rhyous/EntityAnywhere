namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object provide metadata for an entity. This shoudl be changed to implement csdl.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity</typeparam>
    public class EntityMetadata<TEntity>
    {
        /// <summary>
        /// The Entity's web service url.
        /// </summary>
        public string Url
        {
            get { return _Url ?? (_Url = $"{typeof(TEntity).Name}Service.svc"); }
            set { _Url = value; }
        } private string _Url;

        /// <summary>
        /// The Entity's web service help url.
        /// </summary>
        public string HelpUrl
        {
            get { return _HelpUrl ?? (_HelpUrl = $"{Url}/help"); }
            set { _HelpUrl = value; }
        } private string _HelpUrl;

        /// <summary>
        /// The Entity name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// A sample of the entity.
        /// </summary>
        public TEntity ExampleEntity { get; set; }
    }
}
