using System.Collections.Generic;


namespace Rhyous.WebFramework.Interfaces
{
    public interface IPropertyCollection : IList<IProperty>, IEnumerable<IProperty>
    {
        void Add(string property, string value);
    }
}
