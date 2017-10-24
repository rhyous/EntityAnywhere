using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public class PreferentialComparer : IComparer<string>
    {
        public List<string> Preferences = new List<string> { "Id", "Name" };
        public int Compare(string x, string y)
        {
            foreach (var pref in Preferences)
            {
                if (y == pref && x == pref)
                    return 0;
                else if (x == pref)
                    return -1;
                else if (y == pref)
                    return 1;
            }
            return string.Compare(x, y);
        }
    }
}