using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    /// <summary>
    /// Dictionary to map system types to json types for csdl.
    /// </summary>
    public class CsdlTypeDictionary : Dictionary<string, string>
    {
        private static readonly Lazy<CsdlTypeDictionary> Lazy = new Lazy<CsdlTypeDictionary>(() => new CsdlTypeDictionary());

        public static CsdlTypeDictionary Instance => Lazy.Value;

        internal CsdlTypeDictionary()
        {
            Init();
        }

        public void Init()
        {
            Add("System.Boolean", "boolean");
            Add("System.Byte", "integer");
            Add("System.SByte", "integer");
            Add("System.Char", "string");
            Add("System.Decimal", "number");
            Add("System.Double", "number");
            Add("System.Single", "number");
            Add("System.Int32", "integer");
            Add("System.UInt32", "integer");
            Add("System.Int64", "integer");
            Add("System.UInt64", "integer");
            Add("System.Object", "string");
            Add("System.Int16", "integer");
            Add("System.UInt16", "integer");
            Add("System.String", "string");
            Add("System.DateTime", "string");
        }
    }
}
