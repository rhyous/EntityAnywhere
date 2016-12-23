namespace Rhyous.WebFramework.Interfaces
{
    public interface IEntity
    {
    }

    public interface IEntity<Tid> : IEntity, IId<Tid>
    {
    }
}
