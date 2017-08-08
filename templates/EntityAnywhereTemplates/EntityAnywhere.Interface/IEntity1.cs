using System;

namespace Rhyous.WebFramework.Interfaces
{
public interface IEntity1 : IEntity<int>, IName, IDescription, IAuditable
{
}
}
