namespace Rhyous.WebFramework.Behaviors
{
    public static class ServiceResponseExtensions
    {
        public static string GetValue(this ServiceResponseSection section, string key, string defaultValue = "An unknown error occurred.")
        {
            return section.ServiceResponses[key]?.ResponseString ?? defaultValue;
        }
    }
}
