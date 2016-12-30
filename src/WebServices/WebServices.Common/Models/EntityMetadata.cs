namespace Rhyous.WebFramework.WebServices
{
    public class EntityMetadata<T>
    {
        public string Url
        {
            get { return _Url ?? (_Url = $"{typeof(T).Name}Service.svc"); }
            set { _Url = value; }
        } private string _Url;

        public string HelpUrl
        {
            get { return _HelpUrl ?? (_HelpUrl = $"{Url}/help"); }
            set { _HelpUrl = value; }
        } private string _HelpUrl;

        public string EntityName { get; set; }

        public T ExampleEntity { get; set; }
    }
}
