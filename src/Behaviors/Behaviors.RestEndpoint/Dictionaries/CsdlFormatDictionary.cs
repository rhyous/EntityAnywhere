using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    /// <summary>
    /// Dictionary to map system types to json formats for csdl.
    /// </summary>
    public class CsdlFormatDictionary : Dictionary<string, string>
    {
        private static readonly Lazy<CsdlFormatDictionary> Lazy = new Lazy<CsdlFormatDictionary>(() => new CsdlFormatDictionary());

        public static CsdlFormatDictionary Instance => Lazy.Value;

        internal CsdlFormatDictionary()
        {
            Init();
        }

        public void Init()
        {
            Add("System.Boolean", "");
            Add("System.Byte", "uint8");
            Add("System.SByte", "uint8");
            Add("System.Char", "");
            Add("System.Decimal", "decimal");
            Add("System.Double", "double");
            Add("System.Single", "single");
            Add("System.Int32", "int32");
            Add("System.UInt32", "uint32");
            Add("System.Int64", "int64");
            Add("System.UInt64", "uint64");
            Add("System.Object", "");
            Add("System.Int16", "int16");
            Add("System.UInt16", "uint16");
            Add("System.String", "");
            Add("System.DateTime", "date");
        }
        
    }
}
