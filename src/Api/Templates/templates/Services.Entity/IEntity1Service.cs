using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IEntity1Service : IServiceCommon<Entity1, IEntity1, int>
    {
    }
}