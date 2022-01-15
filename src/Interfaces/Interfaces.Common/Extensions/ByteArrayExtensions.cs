using System;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToJson(this byte[] byteArray)
        {
            return $"[{String.Join(",", byteArray.Select(b => b.ToString()))}]";
        }
    }
}