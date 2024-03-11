using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IMissingEntitySettingDetector
    {
        Task<MissingEntitySettings> DetectAsync(IEntitySettingsDictionary settings, IEnumerable<Type> entityTypes);
    }
}