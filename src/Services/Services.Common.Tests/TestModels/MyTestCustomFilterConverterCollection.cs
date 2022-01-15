using Rhyous.Odata.Filter;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    internal class MyTestCustomFilterConverterCollection<T> : ICustomFilterConverterCollection<T>
    {
        public List<IFilterConverter<T>> Converters { get; }
    }
}