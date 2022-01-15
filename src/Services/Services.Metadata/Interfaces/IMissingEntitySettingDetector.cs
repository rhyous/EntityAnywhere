using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IMissingEntitySettingDetector
    {
        MissingEntitySettings Detect(IDictionary<string, EntitySetting> settings, IEnumerable<Type> entityTypes);
    }
}