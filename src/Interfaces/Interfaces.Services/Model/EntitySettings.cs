namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EntitySettings : IEntitySettings
    {
        public string Token { get; set; }
        public string ServiceUrl { get; set; }
        public bool IsAdmin { get; set; }
    }
}