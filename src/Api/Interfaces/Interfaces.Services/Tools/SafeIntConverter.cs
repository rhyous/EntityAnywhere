using Newtonsoft.Json;
using Rhyous.StringLibrary;
using System;
using System.Globalization;
using System.Linq;

namespace Rhyous.EntityAnywhere.Tools
{
    /// <summary>
    /// This class allows for safely conveting a decimal represented as a string to an int.
    /// Newtonsoft.Json currently crashes if a string is a decimal but serialized to an int.
    /// </summary>
    /// <remarks>The is to workaround the calling system, which was a fixed 3rd party system and
    /// could not be changed. It always added a decimal place and always used a string with quotes
    /// in the json: "1.0".</remarks>
    public sealed class SafeIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null || string.IsNullOrWhiteSpace(reader.Value.ToString())) // An int cannot be null
                throw new JsonSerializationException($"Json property {reader.Path} of type {objectType} cannot be null.");
            var strValue = reader.Value.ToString();
            var cultureInfo = CultureInfo.InvariantCulture;
            string separator = cultureInfo.NumberFormat.NumberDecimalSeparator;
            if (strValue.Count(c => c == separator[0]) == 1)
            {
                var d = strValue.To<double>();
                return Convert.ToInt32(d);
            }
            return strValue.ToString().To<int>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
