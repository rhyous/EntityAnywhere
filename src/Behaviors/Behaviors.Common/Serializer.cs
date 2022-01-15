using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Rhyous.StringLibrary;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class Serializer : ISerializer
    {
        public byte[] Json(object obj, IContractResolver resolver = null)
        {
            using (var stream = new MemoryStream())
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false)))
            using (var writer = new JsonTextWriter(sw))
            {
                var serializer = new JsonSerializer()
                {
                    ContractResolver = resolver,
                    DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK",
                    DateParseHandling = DateParseHandling.DateTimeOffset
                };
                serializer.Serialize(writer, obj);
                sw.Flush();
                return stream.ToArray();
            }
        }

        public object Deserialize(byte[] rawBody, Type type)
        {
            using (MemoryStream ms = new MemoryStream(rawBody))
            using (StreamReader sr = new StreamReader(ms))
            {
                var serializer = new JsonSerializer { DateParseHandling = DateParseHandling.DateTimeOffset };
                serializer.Converters.Add(new SafeIntConverter());
                return serializer.Deserialize(sr, type);
            }
        }
    }

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