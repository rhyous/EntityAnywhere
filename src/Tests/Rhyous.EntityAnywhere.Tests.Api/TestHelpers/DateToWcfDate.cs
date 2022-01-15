using Newtonsoft.Json;
using System;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public static class DateToWcfDate
    {
        public static JsonSerializerSettings Settings => new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };

        public static string GetWcfDate(this DateTime dt)
        {
            var json = JsonConvert.SerializeObject(dt, Settings);
            return json.Trim('\"');
        }
        public static string GetWcfDateNoEscape(this DateTime dt)
        {
            var json = JsonConvert.SerializeObject(dt, Settings);
            return json.Trim('\"').Replace("\\/", "/");
        }
    }
}
