using System.Collections.Generic;

namespace Rhyous.WebFramework.Entities
{
    public class CountryComparer : IEqualityComparer<Country>
    {
        public bool Equals(Country left, Country right)
        {
            return left.Id == right.Id;
        }

        public int GetHashCode(Country obj)
        {
            return obj.Id;
        }
    }
}
