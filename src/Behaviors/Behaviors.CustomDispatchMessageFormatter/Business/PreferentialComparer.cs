using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public class PreferentialComparer : IComparer<string>
    {
        #region Singleton
        private static readonly Lazy<PreferentialComparer> Lazy = new Lazy<PreferentialComparer>(() => new PreferentialComparer());
        public static PreferentialComparer Instance { get { return Lazy.Value; } }
        internal PreferentialComparer() { }

        #endregion

        private string First = "Id";
        private string Second = "Name";
        public int Compare(string x, string y)
        {
            if (y == First && x == First)
                return 0;
            else if (x == First)
                return -1;
            else if (y == First)
                return 1;
            else if (y == Second && x == Second)
                return 0;
            else if (x == Second)
                return -1;
            else if (y == Second)
                return 1;
            else
                return string.Compare(x, y);
        }
    }
}
