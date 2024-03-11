using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class PreferentialPropertyComparer : IPreferentialPropertyComparer
    {
        #region Singleton

        private static readonly Lazy<PreferentialPropertyComparer> Lazy = new Lazy<PreferentialPropertyComparer>(() => new PreferentialPropertyComparer());

        /// <summary>This singleton instance</summary>
        public static PreferentialPropertyComparer Instance { get { return Lazy.Value; } }

        internal PreferentialPropertyComparer() { }

        #endregion

        /// <summary>
        /// Properties that should be ordered first by default.
        /// </summary>
        public IList<string> PreferredProperties => new List<string> { { "Id" }, { "Name" } };

        /// <summary>
        /// Properties that should be ordered last by default.
        /// </summary>
        public IList<string> DispreferredProperties => new List<string> { "CreateDate", "CreatedBy", "LastUpdated", "LastUpdatedBy" };

        public int Compare(string x, string y)
        {
            var xPriority = PreferredProperties.IndexOf(x);
            var yPriority = PreferredProperties.IndexOf(y);
            if (xPriority != -1 && yPriority != -1)
                return xPriority.CompareTo(yPriority);
            if (xPriority != -1)
                return -1;
            if (yPriority != -1)
                return 1;
            xPriority = DispreferredProperties.IndexOf(x);
            yPriority = DispreferredProperties.IndexOf(y);
            if (xPriority != -1 && yPriority != -1)
                return xPriority.CompareTo(yPriority);
            if (xPriority != -1)
                return 1;
            if (yPriority != -1)
                return -1;
            return string.Compare(x, y);
        }
    }
}