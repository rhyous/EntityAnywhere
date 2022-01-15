using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IPreferentialPropertyComparer : IComparer<string>
    {
        IList<string> DispreferredProperties { get; }
        IList<string> PreferredProperties { get; }
    }
}